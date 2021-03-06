﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace DBMigrator.Model
{
    public class Script
    {
        public const string MIGRATIONS_UPGRADE_FILENAME_REGEX = @"(\d+)_(?!rollback)(\w+)";
        public const string MIGRATIONS_ROLLBACK_FILENAME_REGEX = @"(\d+)_(rollback)(\w+)";
        public const string CREATE_STORED_PROCEDURE_REGEX = @"CREATE\s+PROCEDURE\s+(\w+)";
        public const string CREATE_FUNCTIONS_REGEX = @"CREATE\s+FUNCTION\s+(\w+)";
        public const string CREATE_TRIGGERS_REGEX = @"CREATE\s+TRIGGER\s+(\w+)";
        public const string CREATE_VIEWS_REGEX = @"CREATE\s+VIEW\s+(\w+)";
        public const string ILLEGAL_REGEX = @"ALTER";

        private List<SQLTYPE> _migrationTypes = new List<SQLTYPE> { SQLTYPE.Upgrade, SQLTYPE.Rollback };

        public enum SQLTYPE { Upgrade, Rollback, View, Function, StoredProcedure, Trigger };

        public Feature Feature { get; }

        public Script(string ScriptFile, int order, SQLTYPE type, Feature feature)
        {
            FileName = ScriptFile;
            Feature = feature;
            Order = order;
            Type = type;

            if (!_migrationTypes.Contains(type))
            {
                if (Regex.IsMatch(SQL, ILLEGAL_REGEX)) throw new Exception($"Not allowed to have ALTER in {type.ToString()} files");
            }
        }


        public int Order { get; }
        public string FileName { get; }
        public SQLTYPE Type { get; }
        public string Checksum { get; set; }
        public int ExecutionTime { get; set; }
        public Script RollbackScript { get; set; }
        public string SQL { get; set; }
    }
}







