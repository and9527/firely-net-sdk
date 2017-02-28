﻿/*
  Copyright (c) 2011-2012, HL7, Inc
  All rights reserved.
  
  Redistribution and use in source and binary forms, with or without modification, 
  are permitted provided that the following conditions are met:
  
   * Redistributions of source code must retain the above copyright notice, this 
     list of conditions and the following disclaimer.
   * Redistributions in binary form must reproduce the above copyright notice, 
     this list of conditions and the following disclaimer in the documentation 
     and/or other materials provided with the distribution.
   * Neither the name of HL7 nor the names of its contributors may be used to 
     endorse or promote products derived from this software without specific 
     prior written permission.
  
  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
  IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
  NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
  POSSIBILITY OF SUCH DAMAGE.
  

*/

using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Support;
using Hl7.Fhir.Introspection;
using System.Diagnostics;

namespace Hl7.Fhir.Model
{
    public partial class ModelInfo
    {
        [System.Diagnostics.DebuggerDisplay(@"\{{DebuggerDisplay,nq}}")] // http://blogs.msdn.com/b/jaredpar/archive/2011/03/18/debuggerdisplay-attribute-best-practices.aspx
        public class SearchParamDefinition
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            [NotMapped]
            private string DebuggerDisplay
            {
                get
                {
                    return String.Format("{0} {1} {2} ({3})", Resource, Name, Type, Expression);
                }
            }

            public string Resource { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
            public string Description { get; set; }
            public SearchParamType Type { get; set; }

            /// <summary>
            /// If this search parameter is a Composite, this array contains 
            /// the list of search parameters the param is a combination of
            /// </summary>
            public string[] CompositeParams { get; set; }

            /// <summary>
            /// One or more paths into the Resource instance that the search parameter 
            /// uses 
            /// </summary>
            public string[] Path { get; set; }

            /// <summary>
            /// The XPath expression for evaluating this search parameter
            /// </summary>
            public string XPath { get; set; }

            /// <summary>
            /// The FHIR Path expresssion that can be used to extract the data
            /// for this search parameter
            /// </summary>
            public string Expression { get; set; }

            /// <summary>
            /// If this is a reference, the possible types of resources that the
            /// parameters references to
            /// </summary>
            public ResourceType[] Target { get; set; }
        }

#if false
        // [WMR 20160421] Slow, based on reflection...
        public static string FhirTypeToFhirTypeName(FHIRAllTypes type)
        {
            return type.GetLiteral();
        }

        // [WMR 20160421] Wrong!
        // FhirTypeToFhirTypeName parses the typename from EnumLiteral attribute on individual FHIRAllTypes member
        // FhirTypeNameToFhirType converts enum member name to FHIRAllTypes enum value
        // Currently, the EnumLiteral attribute value is always equal to the Enum member name and the C# type name
        // However, this is not guaranteed! e.g. a FHIR type name could be a reserved word in C#

        public static FHIRAllTypes? FhirTypeNameToFhirType(string name)
        {
            FHIRAllTypes result; // = FHIRAllTypes.Patient;

            if (Enum.TryParse<FHIRAllTypes>(name, ignoreCase: true, result: out result))
                return result;
            else
                return null;
        }

#else
        // [WMR 20160421] NEW - Improved & optimized
        // 1. Convert from/to FHIR type names as defined by EnumLiteral attributes on FHIRAllTypes enum members
        // 2. Cache lookup tables, to optimize runtime reflection

        /// <summary>Returns the <see cref="FHIRAllTypes"/> enum value that represents the specified FHIR type name, or <c>null</c>.</summary>
        public static string FhirTypeToFhirTypeName(FHIRAllTypes type)
        {
            string result;
            _fhirTypeToFhirTypeName.Value.TryGetValue(type, out result);
            return result;
        }

        private static Lazy<IDictionary<FHIRAllTypes, string>> _fhirTypeToFhirTypeName
            = new Lazy<IDictionary<FHIRAllTypes, string>>(InitFhirTypeToFhirTypeName, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        private static IDictionary<FHIRAllTypes, string> InitFhirTypeToFhirTypeName()
        {
            // Build reverse lookup table
            return _fhirTypeNameToFhirType.Value.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }

        /// <summary>Returns the FHIR type name represented by the specified <see cref="FHIRAllTypes"/> enum value, or <c>null</c>.</summary>
        public static FHIRAllTypes? FhirTypeNameToFhirType(string typeName)
        {
            FHIRAllTypes result;
            if (_fhirTypeNameToFhirType.Value.TryGetValue(typeName, out result))
            {
                return result;
            }
            return null;
        }

        private static Lazy<IDictionary<string, FHIRAllTypes>> _fhirTypeNameToFhirType
            = new Lazy<IDictionary<string, FHIRAllTypes>>(InitFhirTypeNameToFhirType, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        private static IDictionary<string, FHIRAllTypes> InitFhirTypeNameToFhirType()
        {
            var values = Enum.GetValues(typeof(FHIRAllTypes)).OfType<FHIRAllTypes>();
            return values.ToDictionary(type => type.GetLiteral());
        }

#endif

        /// <summary>Returns the C# <see cref="Type"/> that represents the FHIR type with the specified name, or <c>null</c>.</summary>
        public static Type GetTypeForFhirType(string name)
        {
            // [WMR 20160421] Optimization
            //if (!FhirTypeToCsType.ContainsKey(name))
            //    return null;
            //else
            //    return FhirTypeToCsType[name];
            Type result;
            FhirTypeToCsType.TryGetValue(name, out result);
            return result;
        }

        /// <summary>Returns the FHIR type name represented by the specified C# <see cref="Type"/>, or <c>null</c>.</summary>
        public static string GetFhirTypeNameForType(Type type)
        {
            // [WMR 20160421] Optimization
            //if (!FhirCsTypeToString.ContainsKey(type))
            //    return null;
            //else
            //    return FhirCsTypeToString[type];
            string result;
            FhirCsTypeToString.TryGetValue(type, out result);
            return result;
        }

        [Obsolete("Use GetFhirTypeNameForType() instead")]
        public static string GetFhirTypeForType(Type type)
        {
            return GetFhirTypeNameForType(type);
        }

        /// <summary>Determines if the specified value represents the name of a known FHIR resource.</summary>
        public static bool IsKnownResource(string name)
        {
            return SupportedResources.Contains(name);
        }

        /// <summary>Determines if the specified <see cref="Type"/> instance represents a known FHIR resource.</summary>
        public static bool IsKnownResource(Type type)
        {
            var name = GetFhirTypeNameForType(type);

            return name != null && IsKnownResource(name);
        }

        /// <summary>Determines if the specified <see cref="FHIRAllTypes"/> value represents a known FHIR resource.</summary>
        public static bool IsKnownResource(FHIRAllTypes type)
        {
            var name = FhirTypeToFhirTypeName(type);
            return name != null && IsKnownResource(name);
        }

        [Obsolete("Use GetTypeForFhirType() which covers all types, not just resources")]
        public static Type GetTypeForResourceName(string name)
        {
            if (!IsKnownResource(name)) return null;

            return GetTypeForFhirType(name);
        }

        [Obsolete("Use GetFhirTypeNameForType() which covers all types, not just resources")]
        public static string GetResourceNameForType(Type type)
        {
            var name = GetFhirTypeForType(type);

            if (name != null && IsKnownResource(name))
                return name;
            else
                return null;
        }

        /// <summary>Determines if the specified value represents the name of a FHIR primitive data type.</summary>
        public static bool IsPrimitive(string name)
        {
            if (String.IsNullOrEmpty(name)) return false;

            return FhirTypeToCsType.ContainsKey(name) && Char.IsLower(name[0]);
        }

        /// <summary>Determines if the specified <see cref="Type"/> instance represents a FHIR primitive data type.</summary>
        public static bool IsPrimitive(Type type)
        {
            var name = GetFhirTypeNameForType(type);

            return name != null && Char.IsLower(name[0]);
        }

        /// <summary>Determines if the specified <see cref="FHIRAllTypes"/> value represents a FHIR primitive data type.</summary>
        public static bool IsPrimitive(FHIRAllTypes type)
        {
            return IsPrimitive(FhirTypeToFhirTypeName(type));
        }

        /// <summary>Determines if the specified value represents the name of a FHIR complex data type (NOT including resources and primitives).</summary>
        public static bool IsDataType(string name)
        {
            if (String.IsNullOrEmpty(name)) return false;

            return FhirTypeToCsType.ContainsKey(name) && !IsKnownResource(name) && !IsPrimitive(name);
        }

        /// <summary>Determines if the specified <see cref="Type"/> instance represents a FHIR complex data type (NOT including resources and primitives).</summary>
        public static bool IsDataType(Type type)
        {
            var name = GetFhirTypeNameForType(type);

            return name != null && !IsKnownResource(name) && !IsPrimitive(name);
        }

        /// <summary>Determines if the specified <see cref="FHIRAllTypes"/> value represents a FHIR complex data type (NOT including resources and primitives).</summary>
        public static bool IsDataType(FHIRAllTypes type)
        {
            return IsDataType(FhirTypeToFhirTypeName(type));
        }

        // [WMR 20160421] Dynamically resolve FHIR type name 'Reference'
        private static readonly string _referenceTypeName = FHIRAllTypes.Reference.GetLiteral();

        /// <summary>Determines if the specified value represents the type name of a FHIR Reference, i.e. equals "Reference".</summary>
        public static bool IsReference(string name)
        {
            return name == _referenceTypeName; // "Reference";
        }

        /// <summary>Determines if the specified <see cref="Type"/> instance represents a FHIR Reference type.</summary>
        public static bool IsReference(Type type)
        {
            return IsReference(type.Name);
        }

        /// <summary>Determines if the specified <see cref="FHIRAllTypes"/> value represents a FHIR Reference type.</summary>
        public static bool IsReference(FHIRAllTypes type)
        {
            return type == FHIRAllTypes.Reference;
        }

        /// <summary>
        /// Determines if the specified <see cref="FHIRAllTypes"/> value represents a FHIR conformance resource type
        /// (resources under the Conformance/Terminology/Implementation Support header in resourcelist.html)
        /// </summary>
        public static bool IsConformanceResource(Type type)
        {
            return IsConformanceResource(type.Name);
        }

        /// <summary>
        /// Determines if the specified <see cref="FHIRAllTypes"/> value represents a FHIR conformance resource type
        /// (resources under the Conformance/Terminology/Implementation Support header in resourcelist.html)
        /// </summary>
        public static bool IsConformanceResource(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;

            var t = FhirTypeNameToFhirType(name);

            if (t.HasValue)
                return IsConformanceResource(t.Value);
            else
                return false;
        }

        /// <summary>
        /// Determines if the specified <see cref="FHIRAllTypes"/> value represents a FHIR conformance resource type
        /// (resources under the Conformance/Terminology/Implementation Support header in resourcelist.html)
        /// </summary>
        public static bool IsConformanceResource(FHIRAllTypes type)
        {
            return ConformanceResources.Contains(type);
        }



        public static readonly FHIRAllTypes[] ConformanceResources = 
        {
            FHIRAllTypes.StructureDefinition,
            FHIRAllTypes.StructureMap,
            FHIRAllTypes.DataElement,
            FHIRAllTypes.CapabilityStatement,
            FHIRAllTypes.MessageDefinition,
            FHIRAllTypes.OperationDefinition,
            FHIRAllTypes.SearchParameter,
            FHIRAllTypes.CompartmentDefinition,
            FHIRAllTypes.ImplementationGuide,
            FHIRAllTypes.CodeSystem,
            FHIRAllTypes.ValueSet,
            FHIRAllTypes.ConceptMap,
            FHIRAllTypes.ExpansionProfile,
            FHIRAllTypes.NamingSystem,
            FHIRAllTypes.TestScript,
            FHIRAllTypes.TestReport
        };

        /// <summary>Determines if the specified value represents the name of a core Resource, Datatype or primitive.</summary>
        public static bool IsCoreModelType(string name) => FhirTypeToCsType.ContainsKey(name);
            // => IsKnownResource(name) || IsDataType(name) || IsPrimitive(name);

        
        static readonly Uri FhirCoreProfileBaseUri = new Uri(@"http://hl7.org/fhir/StructureDefinition/");

        /// <summary>Determines if the specified value represents the canonical uri of a core Resource, Datatype or primitive.</summary>
        public static bool IsCoreModelTypeUri(Uri uri)
        {
            return uri != null
                && FhirCoreProfileBaseUri.IsBaseOf(uri)
                && IsCoreModelType(FhirCoreProfileBaseUri.MakeRelativeUri(uri).ToString());
        }

        /// <summary>
        /// Returns whether the type has subclasses in the core spec
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks>Quantity is not listed here, since its subclasses are
        /// actually profiles on Quantity. Likewise, there is no real inheritance
        /// in the primitives, so string is not a superclass for markdown</remarks>
        public static bool IsCoreSuperType(FHIRAllTypes type)
        {
            return
                type == FHIRAllTypes.Resource ||
                type == FHIRAllTypes.DomainResource ||
                type == FHIRAllTypes.Element ||
                type == FHIRAllTypes.BackboneElement;
        }

        public static bool IsProfiledQuantity(FHIRAllTypes type)
        {
            return
                type == FHIRAllTypes.Age ||
                type == FHIRAllTypes.Distance ||
                type == FHIRAllTypes.SimpleQuantity ||
                type == FHIRAllTypes.Duration ||
                type == FHIRAllTypes.Count ||
                type == FHIRAllTypes.Money;
        }

        public static bool IsInstanceTypeFor(FHIRAllTypes superclass, FHIRAllTypes subclass)
        {
            if (superclass == subclass) return true;

            if (IsKnownResource(subclass))
            {
                if (superclass == FHIRAllTypes.Resource)
                    return true;
                else if (superclass == FHIRAllTypes.DomainResource)
                    return subclass != FHIRAllTypes.Parameters && subclass != FHIRAllTypes.Bundle && subclass != FHIRAllTypes.Binary;
                else
                    return false;
            }
            else
                return superclass == FHIRAllTypes.Element;
        }

        public static string CanonicalUriForFhirCoreType(string typename)
        {
            return "http://hl7.org/fhir/StructureDefinition/" + typename;
        }

        public static string CanonicalUriForFhirCoreType(FHIRAllTypes type)
        {
            return CanonicalUriForFhirCoreType(type.GetLiteral());
        }

    }

    public static class ModelInfoExtensions
    {
        public static string GetCollectionName(this Type type)
        {
            if (type.CanBeTreatedAsType(typeof(Resource)))
                return ModelInfo.GetFhirTypeNameForType(type);
            else
                throw new ArgumentException(String.Format(
                    "Cannot determine collection name, type {0} is not a resource type", type.Name));
        }
    }

}