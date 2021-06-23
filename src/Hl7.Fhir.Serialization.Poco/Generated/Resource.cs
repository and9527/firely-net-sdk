// <auto-generated/>
// Contents of: hl7.fhir.r4.core version: 4.0.1

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;

/*
  Copyright (c) 2011+, HL7, Inc.
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

namespace Hl7.Fhir.Serialization.Poco
{
  /// <summary>
  /// JSON Serialization Extensions for Resource
  /// </summary>
  public static class ResourceSerializationExtensions
  {
    /// <summary>
    /// Serialize a FHIR Resource into JSON
    /// </summary>
    public static void SerializeJson(this Hl7.Fhir.Model.Resource current, Utf8JsonWriter writer, JsonSerializerOptions options, bool includeStartObject = true)
    {
      if (includeStartObject) { writer.WriteStartObject(); }
      JsonStreamUtilities.SerializePrimitiveProperty("id",current.IdElement,writer,options);

      JsonStreamUtilities.SerializeComplexProperty("meta", current.Meta, writer, options);

      JsonStreamUtilities.SerializePrimitiveProperty("implicitRules",current.ImplicitRulesElement,writer,options);

      JsonStreamUtilities.SerializePrimitiveProperty("language",current.LanguageElement,writer,options);

      if (includeStartObject) { writer.WriteEndObject(); }
    }

    /// <summary>
    /// Deserialize JSON into a FHIR Resource
    /// </summary>
    public static void DeserializeJson(this Hl7.Fhir.Model.Resource current, ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
      string propertyName;

      while (reader.Read())
      {
        if (reader.TokenType == JsonTokenType.EndObject)
        {
          return;
        }

        if (reader.TokenType == JsonTokenType.PropertyName)
        {
          propertyName = reader.GetString();
          if (Hl7.Fhir.Serialization.FhirSerializerOptions.Debug) { Console.WriteLine($"Resource >>> Resource.{propertyName}, depth: {reader.CurrentDepth}, pos: {reader.BytesConsumed}"); }
          reader.Read();
          current.DeserializeJsonProperty(ref reader, options, propertyName);
        }
      }

      throw new JsonException($"Resource: invalid state! depth: {reader.CurrentDepth}, pos: {reader.BytesConsumed}");
    }

    /// <summary>
    /// Deserialize JSON into a FHIR Resource
    /// </summary>
    public static void DeserializeJsonProperty(this Hl7.Fhir.Model.Resource current, ref Utf8JsonReader reader, JsonSerializerOptions options, string propertyName)
    {
      switch (propertyName)
      {
        case "id":
          if (reader.TokenType == JsonTokenType.Null)
          {
            current.IdElement = new Id();
            reader.Skip();
          }
          else
          {
            current.IdElement = new Id(reader.GetString());
          }
          break;

        case "_id":
          if (current.IdElement == null) { current.IdElement = new Id(); }
          ((Hl7.Fhir.Model.Element)current.IdElement).DeserializeJson(ref reader, options);
          break;

        case "meta":
          current.Meta = new Hl7.Fhir.Model.Meta();
          ((Hl7.Fhir.Model.Meta)current.Meta).DeserializeJson(ref reader, options);
          break;

        case "implicitRules":
          if (reader.TokenType == JsonTokenType.Null)
          {
            current.ImplicitRulesElement = new FhirUri();
            reader.Skip();
          }
          else
          {
            current.ImplicitRulesElement = new FhirUri(reader.GetString());
          }
          break;

        case "_implicitRules":
          if (current.ImplicitRulesElement == null) { current.ImplicitRulesElement = new FhirUri(); }
          ((Hl7.Fhir.Model.Element)current.ImplicitRulesElement).DeserializeJson(ref reader, options);
          break;

        case "language":
          if (reader.TokenType == JsonTokenType.Null)
          {
            current.LanguageElement = new Code();
            reader.Skip();
          }
          else
          {
            current.LanguageElement = new Code(reader.GetString());
          }
          break;

        case "_language":
          if (current.LanguageElement == null) { current.LanguageElement = new Code(); }
          ((Hl7.Fhir.Model.Element)current.LanguageElement).DeserializeJson(ref reader, options);
          break;

      }
    }

  }

}

// end of file