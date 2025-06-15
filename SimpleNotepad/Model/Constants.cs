using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNotepad.Model
{
    [Flags]
    public enum CodeNamingConvention
    {
        CamelCase,
        CamelCaseFirstCapital,
        Prefix
    }

    public enum CodeFieldType
    {
        Value,
        Pointer,
        UnboundedArray,
        BoundedArray
    }

    public enum CodeArgumentType
    {
        Value,
        Reference,
        Pointer,
        UnboundedArray,
        BoundedArray
    }

    [Flags]
    public enum CodeModifier
    {
        None = 0,
        Const,
        Static,
        Virtual,
        Override
    }

    public enum CodeAccess
    {
        Public,
        Private,
        Protected
    }

    public enum CodeTemplateType
    {
        Class,
        Specializer
    }
}
