//----------------------------------------------------------------------- 
// PDS.Witsml.Server, 2016.1
//
// Copyright 2016 Petrotechnical Data Systems
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

// ----------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost
//     if the code is regenerated.
// </auto-generated>
// ----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Energistics.DataAccess.WITSML141;
using Energistics.DataAccess.WITSML141.ComponentSchemas;
using Energistics.Datatypes;
using LinqToQuerystring;
using PDS.Framework;
using PDS.Witsml.Server.Configuration;

namespace PDS.Witsml.Server.Data.Logs
{
    /// <summary>
    /// Data adapter that encapsulates CRUD functionality for <see cref="Log" />
    /// </summary>
    /// <seealso cref="PDS.Witsml.Server.Data.Logs.LogDataAdapter{Log,LogCurveInfo}" />
    [Export(typeof(IWitsml141Configuration))]
    [Export(typeof(IWitsmlDataAdapter<Log>))]
    [Export141(ObjectTypes.Log, typeof(IWitsmlDataAdapter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Log141DataAdapter : LogDataAdapter<Log, LogCurveInfo>, IWitsml141Configuration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Log141DataAdapter" /> class.
        /// </summary>
        /// <param name="container">The composition container.</param>
        /// <param name="databaseProvider">The database provider.</param>
        [ImportingConstructor]
        public Log141DataAdapter(IContainer container, IDatabaseProvider databaseProvider)
            : base(container, databaseProvider, ObjectNames.Log141)
        {
            Logger.Debug("Instance created.");
        }

        /// <summary>
        /// Gets the supported capabilities for the <see cref="Log"/> object.
        /// </summary>
        /// <param name="capServer">The capServer instance.</param>
        public void GetCapabilities(CapServer capServer)
        {
            Logger.DebugFormat("Getting the supported capabilities for Log data version {0}.", capServer.Version);

            capServer.Add(Functions.GetFromStore, ObjectTypes.Log, WitsmlSettings.LogMaxDataNodesGet, WitsmlSettings.LogMaxDataPointsGet);
            capServer.Add(Functions.AddToStore, ObjectTypes.Log, WitsmlSettings.LogMaxDataNodesAdd, WitsmlSettings.LogMaxDataPointsAdd);
            capServer.Add(Functions.UpdateInStore, ObjectTypes.Log, WitsmlSettings.LogMaxDataNodesUpdate, WitsmlSettings.LogMaxDataPointsUpdate);
            capServer.Add(Functions.DeleteFromStore, ObjectTypes.Log, WitsmlSettings.LogMaxDataNodesDelete, WitsmlSettings.LogMaxDataPointsDelete);
            capServer.SetGrowingTimeoutPeriod(ObjectTypes.Log, WitsmlSettings.LogGrowingTimeoutPeriod);
      }

        /// <summary>
        /// Gets a collection of data objects related to the specified URI.
        /// </summary>
        /// <param name="parentUri">The parent URI.</param>
        /// <returns>A collection of data objects.</returns>
        public override List<Log> GetAll(EtpUri? parentUri)
        {
            Logger.DebugFormat("Fetching all Logs; Parent URI: {0}", parentUri);

            return GetAllQuery(parentUri)
                .OrderBy(x => x.Name)
                .ToList();
        }

        /// <summary>
        /// Gets an <see cref="IQueryable{Log}" /> instance to by used by the GetAll method.
        /// </summary>
        /// <param name="parentUri">The parent URI.</param>
        /// <returns>An executable query.</returns>
        protected override IQueryable<Log> GetAllQuery(EtpUri? parentUri)
        {
            var query = GetQuery().AsQueryable();

            if (parentUri != null)
            {
                var ids = parentUri.Value.GetObjectIds().ToDictionary(x => x.ObjectType, y => y.ObjectId, StringComparer.CurrentCultureIgnoreCase);
                var uidWellbore = ids[ObjectTypes.Wellbore];
                var uidWell = ids[ObjectTypes.Well];

                if (!string.IsNullOrWhiteSpace(uidWell))
                    query = query.Where(x => x.UidWell == uidWell);

                if (!string.IsNullOrWhiteSpace(uidWellbore))
                    query = query.Where(x => x.UidWellbore == uidWellbore);

                if (!string.IsNullOrWhiteSpace(parentUri.Value.Query))
                    query = query.LinqToQuerystring(parentUri.Value.Query);
            }

            return query;
        }
    }
}