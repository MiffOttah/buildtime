using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Reflection;

namespace BuildTime
{
    static class CodeGen
    {
        public static string GenerateCode(BuildTimeArgs args, DateTimeOffset buildTime)
        {
            var unit = new CodeCompileUnit();
            var ns = string.IsNullOrEmpty(args.Namespace) ? new CodeNamespace() : new CodeNamespace(args.Namespace);
            var type = GenerateType(args, buildTime);
            ns.Types.Add(type);
            unit.Namespaces.Add(ns);

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions
            {
                BracingStyle = "C"
            };

            using (var w = new System.IO.StringWriter())
            {
                provider.GenerateCodeFromCompileUnit(unit, w, options);
                return w.ToString();
            }
        }

        private static CodeTypeDeclaration GenerateType(BuildTimeArgs args, DateTimeOffset buildTime)
        {
            string buildTimeString = buildTime.ToString("O", CultureInfo.InvariantCulture);
            var parseExact = new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression(typeof(DateTimeOffset)),
                "ParseExact",
                new CodePrimitiveExpression(buildTimeString),
                new CodePrimitiveExpression("O"),
                new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(CultureInfo)), "InvariantCulture")
            );

            var returnStatement = new CodeMethodReturnStatement(parseExact);

            var property = new CodeMemberProperty
            {
                Attributes = MemberAttributes.Final | MemberAttributes.Public | MemberAttributes.Static,
                HasGet = true,
                HasSet = false,
                Name = "BuildTime",
                Type = new CodeTypeReference(typeof(DateTimeOffset)),
            };
            property.GetStatements.Add(new CodeCommentStatement(buildTime.ToString("G")));
            property.GetStatements.Add(returnStatement);

            var type = new CodeTypeDeclaration(args.Class)
            {
                IsClass = true,
                Attributes = MemberAttributes.Static,
                TypeAttributes = args.Public ? TypeAttributes.Public : TypeAttributes.NotPublic
            };

            type.Members.Add(property);
            return type;
        }
    }
}
