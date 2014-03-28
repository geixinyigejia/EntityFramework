// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.Data.Migrations.Utilities;
using Microsoft.Data.Relational;

namespace Microsoft.Data.Migrations.Model
{
    public class DropIndexOperation : MigrationOperation
    {
        private readonly SchemaQualifiedName _tableName;
        private readonly string _indexName;

        public DropIndexOperation(SchemaQualifiedName tableName, [NotNull] string indexName)
        {
            Check.NotNull(indexName, "indexName");

            _tableName = tableName;
            _indexName = indexName;
        }

        public virtual SchemaQualifiedName TableName
        {
            get { return _tableName; }
        }

        public virtual string IndexName
        {
            get { return _indexName; }
        }

        public override void Accept([NotNull] MigrationOperationVisitor visitor)
        {
            Check.NotNull(visitor, "visitor");

            visitor.Visit(this);
        }
    }
}