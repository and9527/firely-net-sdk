﻿/* 
 * Copyright (c) 2016, Firely (info@fire.ly) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.githubusercontent.com/FirelyTeam/fhir-net-api/master/LICENSE
 */

using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Model;
using Hl7.FhirPath;
using System;

namespace Hl7.Fhir.FhirPath
{
    public class FhirEvaluationContext : EvaluationContext
    {
        /// <summary>Creates a new <see cref="FhirEvaluationContext"/> instance with default property values.</summary>
        public static new FhirEvaluationContext CreateDefault() => new FhirEvaluationContext();

        /// <summary>Default constructor. Creates a new <see cref="FhirEvaluationContext"/> instance with default property values.</summary>
        public FhirEvaluationContext() : base()
        {
        }

        public FhirEvaluationContext(Resource context) : base(context?.ToTypedElement())
        {
        }

        public FhirEvaluationContext(ITypedElement context) : base(context)
        {
        }

        #region Obsolote members
        [Obsolete("Please use CreateDefault() instead of this member, which may cause raise conditions. Obsolete since 2018-10-17")]
        new public static readonly FhirEvaluationContext Default = new FhirEvaluationContext();
        #endregion

        private Func<string, ITypedElement> _elementResolver;

        public Func<string, ITypedElement> ElementResolver
        {
            get { return _elementResolver; }
            set { _elementResolver = value; }
        }
    }
}
