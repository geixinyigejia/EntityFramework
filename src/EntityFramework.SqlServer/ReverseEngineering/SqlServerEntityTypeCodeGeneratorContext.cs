﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Relational.Design.ReverseEngineering;
using Microsoft.Data.Entity.Utilities;

namespace Microsoft.Data.Entity.SqlServer.ReverseEngineering
{
    public class SqlServerEntityTypeCodeGeneratorContext : EntityTypeCodeGeneratorContext
    {
        public SqlServerEntityTypeCodeGeneratorContext(
            [NotNull]IEntityType entityType,
            [CanBeNull]string namespaceName,
            [NotNull]DbContextCodeGeneratorContext contextCodeGeneratorContext)
            : base(entityType, namespaceName, contextCodeGeneratorContext)
        {
        }

        public override void GenerateEntityProperty(IndentedStringBuilder sb, IProperty property)
        {
            GenerateEntityPropertyAttribues(sb, property);
            sb.Append("public ");
            //TODO workaround for property.PropertyType.IsGenericType being missing in ASPNETCORE50
            bool isNullableType;
            try
            {
                isNullableType = (typeof(Nullable<>) == property.PropertyType.GetGenericTypeDefinition());
            }
            catch (InvalidOperationException)
            {
                isNullableType = false;
            }
            if (isNullableType)
            {
                sb.Append(Nullable.GetUnderlyingType(property.PropertyType).Name + "?");
            }
            else
            {
                sb.Append(property.PropertyType.Name);
            }
            sb.Append(" ");
            sb.Append(PropertyToPropertyNameMap[property]);
            sb.AppendLine(" { get; set; }");
        }

        public override void GenerateEntityNavigation(IndentedStringBuilder sb, INavigation navigation)
        {
            //TODO
        }

        public virtual void GenerateEntityPropertyAttribues(IndentedStringBuilder sb, IProperty property)
        {
            if (property.IsKey())
            {
                string ordinal = string.Empty;
                var primaryKeyOrdinalPositionAnnotation =
                      ((Property)property).TryGetAnnotation(SqlServerMetadataModelProvider.AnnotationNamePrimaryKeyOrdinal);
                if (primaryKeyOrdinalPositionAnnotation != null)
                {
                    ordinal = "(Ordinal = " + primaryKeyOrdinalPositionAnnotation.Value + ")";
                }
                sb.AppendLine("[Key" + ordinal + "]");
            }

            foreach (var annotation in property.Annotations)
            {
                sb.AppendLine("// Annotation[" + annotation.Name + "] = >>>" + annotation.Value + "<<<");
            }
        }
    }
}