using System.Text;

using SimpleNotepad.Model;

using SimpleWpf.Extensions;
using SimpleWpf.Extensions.Collection;

namespace SimpleNotepad.Component
{
    public static class CPlusPlusCodeGenerator
    {
        public static string CreateClass(CodeClass cclass)
        {
            var builder = new StringBuilder();

            // Procedure: Create pieces -> Assemble pieces
            //
            // .h
            // 0) Create Template Header (if required)
            // 1) Create Class Declaration
            // 2) Create public, protected, private fields, function headers
            //
            // .cpp
            // 3) Create function signatures
            // 4) Create function bodies (for default functions)
            //
            // Assemble:
            //
            // 5) Scope the function bodies (using curly brackets)
            // 6) Scope the class .h members (using curly brackets)
            //      -> Adding public, protected, private labels
            //
            // 7) Scope the whole thing in the namespace enclosing brackets
            //

            var classTemplate = GenerateClassTemplateDeclarator(cclass.ContainerTemplates);
            var classDeclaration = "class " + cclass.Name;

            var privateFieldDeclarations = cclass.Fields.Where(x => x.Access == CodeAccess.Private).Select(x => GenerateField(x, true)).Actualize();
            var protectedFieldDeclarations = cclass.Fields.Where(x => x.Access == CodeAccess.Protected).Select(x => GenerateField(x, true)).Actualize();
            var publicFieldDeclarations = cclass.Fields.Where(x => x.Access == CodeAccess.Public).Select(x => GenerateField(x, true)).Actualize();

            var defaultFunctionDeclarations = cclass.DefaultFunctions.Select(x => GenerateFunctionSignature(cclass, x, true)).Actualize();
            var privateFunctionDeclarations = cclass.Functions.Where(x => x.Access == CodeAccess.Private).Select(x => GenerateFunctionSignature(cclass, x, true)).Actualize();
            var protectedFunctionDeclarations = cclass.Functions.Where(x => x.Access == CodeAccess.Protected).Select(x => GenerateFunctionSignature(cclass, x, true)).Actualize();
            var publicFunctionDeclarations = cclass.Functions.Where(x => x.Access == CodeAccess.Public).Select(x => GenerateFunctionSignature(cclass, x, true)).Actualize();

            // Namespace
            builder.AppendLine(string.Format("namespace {0}", cclass.Namespace));
            builder.AppendLine("{");

            // .h
            // (Indent) Class Template
            if (cclass.IsContainer)
                builder.AppendLine("\t" + classTemplate);

            // (Indent) Class
            builder.AppendLine("\t" + classDeclaration);
            builder.AppendLine("\t{");

            if (defaultFunctionDeclarations.Any())
            {
                // (Indent)
                builder.AppendLine("\tpublic:");                // Default Functions are public

                // (Indent)(Indent) Default Functions
                foreach (var function in defaultFunctionDeclarations)
                    builder.AppendLine("\t\t" + function);

                builder.AppendLine("");
            }

            if (publicFunctionDeclarations.Any())
            {
                // (Indent)
                builder.AppendLine("\tpublic:");

                // (Indent)(Indent) Public Functions
                foreach (var function in publicFunctionDeclarations)
                    builder.AppendLine("\t\t" + function);

                builder.AppendLine("");
            }

            if (protectedFunctionDeclarations.Any())
            {
                // (Indent)
                builder.AppendLine("\tprotected:");

                // (Indent)(Indent) Protected Functions
                foreach (var function in protectedFunctionDeclarations)
                    builder.AppendLine("\t\t" + function);

                builder.AppendLine("");
            }

            if (privateFunctionDeclarations.Any())
            {
                // (Indent)
                builder.AppendLine("\tprivate:");

                // (Indent)(Indent) Private Functions
                foreach (var function in privateFunctionDeclarations)
                    builder.AppendLine("\t\t" + function);

                builder.AppendLine("");
            }

            if (publicFieldDeclarations.Any())
            {
                // (Indent)
                builder.AppendLine("\tpublic:");

                // (Indent)(Indent) Public Fields
                foreach (var field in publicFieldDeclarations)
                    builder.AppendLine("\t\t" + field);

                builder.AppendLine("");
            }

            if (protectedFieldDeclarations.Any())
            {
                // (Indent)
                builder.AppendLine("\tprotected:");

                // (Indent)(Indent) Protected Fields
                foreach (var field in protectedFieldDeclarations)
                    builder.AppendLine("\t\t" + field);

                builder.AppendLine("");
            }

            if (privateFieldDeclarations.Any())
            {
                builder.AppendLine("\tprivate:");

                // (Indent)(Indent) Private Fields
                foreach (var field in privateFieldDeclarations)
                    builder.AppendLine("\t\t" + field);
            }

            // (Indent) End Class
            builder.AppendLine("\t};");

            // .cpp

            if (defaultFunctionDeclarations.Any())
            {
                builder.AppendLine("");
                builder.AppendLine("#pragma region Default Functions");
                builder.AppendLine("");

                // (Indent) Default Functions
                foreach (var function in defaultFunctionDeclarations)
                {
                    builder.AppendLine("\t" + function);
                    builder.AppendLine("\t{");
                    builder.AppendLine("\t}");
                }

                builder.AppendLine("");
                builder.AppendLine("#pragma endregion");
                builder.AppendLine("");
            }

            if (publicFunctionDeclarations.Any())
            {
                builder.AppendLine("");
                builder.AppendLine("#pragma region Public Functions");
                builder.AppendLine("");

                // (Indent) Public Functions
                foreach (var function in publicFunctionDeclarations)
                {
                    builder.AppendLine("\t" + function);
                    builder.AppendLine("\t{");
                    builder.AppendLine("\t}");
                }

                builder.AppendLine("");
                builder.AppendLine("#pragma endregion");
                builder.AppendLine("");
            }

            if (protectedFunctionDeclarations.Any())
            {
                builder.AppendLine("");
                builder.AppendLine("#pragma region Protected Functions");
                builder.AppendLine("");

                // (Indent) Protected Functions
                foreach (var function in protectedFunctionDeclarations)
                {
                    builder.AppendLine("\t" + function);
                    builder.AppendLine("\t{");
                    builder.AppendLine("\t}");
                }

                builder.AppendLine("");
                builder.AppendLine("#pragma endregion");
                builder.AppendLine("");
            }

            if (privateFunctionDeclarations.Any())
            {
                builder.AppendLine("");
                builder.AppendLine("#pragma region Private Functions");
                builder.AppendLine("");

                // (Indent) Private Functions
                foreach (var function in privateFunctionDeclarations)
                {
                    builder.AppendLine("\t" + function);
                    builder.AppendLine("\t{");
                    builder.AppendLine("\t}");
                }

                builder.AppendLine("");
                builder.AppendLine("#pragma endregion");
            }

            // End Namespace
            builder.AppendLine("}");

            return builder.ToString();
        }

        /// <summary>
        /// Generates an argument type that is default for a return value.
        /// </summary>
        public static CodeArgument CreateDefaultReturnArgument(CodeField field)
        {
            var result = new CodeArgument()
            {
                BoundedArrayLengthVariable = field.BoundedArrayLengthVariable,
                FieldType = field.FieldType,
                //Modifier = field.Modifier,
                Name = field.Name,
                Type = field.Type
            };

            // Use defaults from the class field settings
            result.ArgumentType = result.DefaultArgumentType;

            return result;
        }

        public static string GenerateField(CodeField field, bool applyEndlineCharacter = true)
        {
            if (field.GetType() == typeof(CodeContainerField))
            {
                return CreateCodeContainerField((CodeContainerField)field, applyEndlineCharacter);
            }
            else if (field.GetType() == typeof(CodeTemplateField))
            {
                return CreateCodeTemplateField((CodeTemplateField)field, applyEndlineCharacter);
            }
            else if (field.GetType() == typeof(CodeField))
            {
                return CreateCodeField(field, applyEndlineCharacter);
            }
            else if (field.GetType() == typeof(CodeArgument))
            {
                return CreateCodeField(field, applyEndlineCharacter);
            }
            else
                throw new Exception("Unhandled CodeField type:  CodeGenerator.cs");
        }
        public static string GenerateFieldAsReturn(CodeField field)
        {
            if (field.GetType() == typeof(CodeContainerField))
            {
                return CreateCodeContainerFieldAsReturn((CodeContainerField)field);
            }
            else if (field.GetType() == typeof(CodeTemplateField))
            {
                return CreateCodeTemplateFieldAsReturn((CodeTemplateField)field);
            }
            else if (field.GetType() == typeof(CodeField))
            {
                return CreateCodeFieldAsReturn(field);
            }
            else
                throw new Exception("Unhandled CodeField type:  CodeGenerator.cs");
        }

        #region Function Methods
        public static string GenerateFunctionSignature(CodeClass cclass, CodeFunction function, bool applyEndlineCharacter = true)
        {
            // Modifier
            var modifiers = CreateFunctionModifier(function.Modifier);

            // Return Value (Check for ctor)
            var returnValue = function.Return != null ? GenerateFieldAsReturn(function.Return) : "";

            // Declarator
            var functionDeclarator = "";
            var classTemplates = GenerateTemplateDeclarator(cclass.ContainerTemplates);

            if (!cclass.IsContainer)
                functionDeclarator = cclass.Name + "::" + function.Name;

            else
                functionDeclarator = cclass.Name + classTemplates + "::" + function.Name;

            var endline = applyEndlineCharacter ? ";" : "";

            // Arguments
            string argumentList = function.Arguments.Join(",", argument =>
            {
                return GenerateField(argument, false);
            });

            if (function.Return != null)
            {
                if (!string.IsNullOrWhiteSpace(modifiers))
                    return string.Format("{0} {1} {2}{3}({4}){5}", modifiers, returnValue, function.Prefix, functionDeclarator, argumentList, endline);

                else
                    return string.Format("{0} {1}{2}({3}){4}", returnValue, function.Prefix, functionDeclarator, argumentList, endline);
            }

            else
            {
                if (!string.IsNullOrWhiteSpace(modifiers))
                    return string.Format("{0} {1}{2}({3}){4}", modifiers, function.Prefix, functionDeclarator, argumentList, endline);

                else
                    return string.Format("{0}{1}({2}){3}", function.Prefix, functionDeclarator, argumentList, endline);
            }
        }
        public static string GenerateTemplateDeclarator(IEnumerable<CodeTemplate> templates)
        {
            return "<" + templates.Join(",", x => x.Variable) + ">";
        }
        public static string GenerateClassTemplateDeclarator(IEnumerable<CodeTemplate> templates)
        {
            return "template<" + templates.Join(",", x => x.Constraint + " " + x.Variable) + ">";
        }
        #endregion

        #region Helper Methods
        public static string CreateFunctionModifier(CodeModifier modifier)
        {
            return CreateFieldModifier(modifier);
        }
        public static string CreateFieldModifier(CodeModifier modifier)
        {
            var result = "";

            if (modifier.Has(CodeModifier.Virtual))
            {
                result += "virtual ";
            }
            if (modifier.Has(CodeModifier.Static))
            {
                result += "static ";
            }
            if (modifier.Has(CodeModifier.Const))
            {
                result += "const ";
            }

            return result.Trim();
        }
        public static string CreateFieldAsReturnModifier(CodeModifier modifier)
        {
            if (modifier.Has(CodeModifier.Const))
            {
                return "const";
            }
            return "";
        }
        #endregion

        #region Return Value Methods
        private static string CreateCodeContainerFieldAsReturn(CodeContainerField field)
        {
            var result = CreateFieldAsReturnModifier(field.Modifier);

            // Join the template arguments
            var templateString = field.Templates.Join(",", (x) =>
            {
                return x;
            });

            switch (field.FieldType)
            {
                case CodeFieldType.Pointer:
                    result += string.Format("{0}<{1}>*", field.Type, templateString);
                    break;
                case CodeFieldType.BoundedArray:
                    result += string.Format("{0}<{1}>[{2}]", field.Type, templateString, field.BoundedArrayLengthVariable);
                    break;
                case CodeFieldType.UnboundedArray:
                    result += string.Format("{0}<{1}>[]", field.Type, templateString);
                    break;
                case CodeFieldType.Value:
                    result += string.Format("{0}<{1}>", field.Type, templateString);
                    break;
                default:
                    throw new Exception("Unhandled CodeFieldType:  CodeField.cs");
            }

            return result;
        }
        private static string CreateCodeFieldAsReturn(CodeField field)
        {
            var builder = new StringBuilder();

            builder.Append(CreateFieldAsReturnModifier(field.Modifier));

            switch (field.FieldType)
            {
                case CodeFieldType.Pointer:
                    builder.Append(string.Format("{0}*", field.Type));
                    break;
                case CodeFieldType.BoundedArray:
                    builder.Append(string.Format("{0}[{1}]", field.Type, field.BoundedArrayLengthVariable));
                    break;
                case CodeFieldType.UnboundedArray:
                    builder.Append(string.Format("{0}[]", field.Type));
                    break;
                case CodeFieldType.Value:
                    builder.Append(string.Format("{0}", field.Type));
                    break;
                default:
                    throw new Exception("Unhandled CodeFieldType:  CodeField.cs");
            }

            return builder.ToString();
        }
        private static string CreateCodeTemplateFieldAsReturn(CodeTemplateField field)
        {
            return CreateCodeFieldAsReturn(field);
        }
        #endregion

        #region Field Methods
        private static string CreateCodeContainerField(CodeContainerField field, bool applyEndlineCharacter)
        {
            var builder = new StringBuilder();

            builder.Append(CreateFieldModifier(field.Modifier));

            // Join the template arguments
            string templateString = field.Templates.Join(",", (x) =>
            {
                return x;
            });

            var endline = applyEndlineCharacter ? ";" : "";

            switch (field.FieldType)
            {
                case CodeFieldType.Pointer:
                    builder.Append(string.Format("{0}<{1}>* {2}{3}", field.Type, templateString, field.Name, endline));
                    break;
                case CodeFieldType.BoundedArray:
                    builder.Append(string.Format("{0}<{1}> {2}[{3}]{4}", field.Type, templateString, field.Name, field.BoundedArrayLengthVariable, endline));
                    break;
                case CodeFieldType.UnboundedArray:
                    builder.Append(string.Format("{0}<{1}> {2}[]{3}", field.Type, templateString, field.Name, endline));
                    break;
                case CodeFieldType.Value:
                    builder.Append(string.Format("{0}<{1}> {2}{3}", field.Type, templateString, field.Name, endline));
                    break;
                default:
                    throw new Exception("Unhandled CodeFieldType:  CodeField.cs");
            }

            return builder.ToString();
        }
        private static string CreateCodeTemplateField(CodeTemplateField field, bool applyEndlineCharacter)
        {
            return CreateCodeField(field, applyEndlineCharacter);
        }
        private static string CreateCodeField(CodeField field, bool applyEndlineCharacter)
        {
            var builder = new StringBuilder();

            builder.Append(CreateFieldModifier(field.Modifier));

            var endline = applyEndlineCharacter ? ";" : "";

            switch (field.FieldType)
            {
                case CodeFieldType.Pointer:
                    builder.Append(string.Format("{0}* {1}{2}", field.Type, field.Name, endline));
                    break;
                case CodeFieldType.BoundedArray:
                    builder.Append(string.Format("{0} {1}[{2}]{3}", field.Type, field.Name, field.BoundedArrayLengthVariable, endline));
                    break;
                case CodeFieldType.UnboundedArray:
                    builder.Append(string.Format("{0} {1}[]{2}", field.Type, field.Name, endline));
                    break;
                case CodeFieldType.Value:
                    builder.Append(string.Format("{0} {1}{2}", field.Type, field.Name, endline));
                    break;
                default:
                    throw new Exception("Unhandled CodeFieldType:  CodeField.cs");
            }

            return builder.ToString();
        }
        #endregion
    }
}
