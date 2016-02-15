﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cci = Microsoft.Cci;

namespace Pchp.CodeAnalysis
{
    internal partial class Symbol : Cci.IReference
    {
        /// <summary>
        /// Checks if this symbol is a definition and its containing module is a SourceModuleSymbol.
        /// </summary>
        [Conditional("DEBUG")]
        internal protected void CheckDefinitionInvariant()
        {
            //// can't be generic instantiation
            //Debug.Assert(this.IsDefinition);

            //// must be declared in the module we are building
            //Debug.Assert(this.ContainingModule is SourceModuleSymbol ||
            //             (this.Kind == SymbolKind.Assembly && this is SourceAssemblySymbol) ||
            //             (this.Kind == SymbolKind.NetModule && this is SourceModuleSymbol));
        }

        Cci.IDefinition Cci.IReference.AsDefinition(EmitContext context)
        {
            throw new NotSupportedException();
        }

        void Cci.IReference.Dispatch(Cci.MetadataVisitor visitor)
        {
            throw new NotSupportedException();
        }

        IEnumerable<Cci.ICustomAttribute> Cci.IReference.GetAttributes(EmitContext context)
        {
            return ImmutableArray<Cci.ICustomAttribute>.Empty; // GetCustomAttributesToEmit(((PEModuleBuilder)context.Module).CompilationState);
        }
    }
}
