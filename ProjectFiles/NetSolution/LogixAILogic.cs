#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.EventLogger;
using FTOptix.HMIProject;
using FTOptix.Alarm;
using FTOptix.UI;
using FTOptix.WebUI;
using FTOptix.DataLogger;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.ODBCStore;
using FTOptix.OPCUAClient;
using FTOptix.RAEtherNetIP;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.CommunicationDriver;
using FTOptix.NetLogic;
using FTOptix.Core;
using System.Xml.Linq;
using System.Text.RegularExpressions;
#endregion

public class LogixAILogic : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started

        myPeriodicTask = new PeriodicTask(FTMetricsReadData, 10000, LogicObject);
        myPeriodicTask.Start();

    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void FTMetricsReadData()
    {
        TotalAlarmCountQuery();
        TotalAckedAlarmCountQuery();
        TotalConfirmedAlarmCountQuery();
        Mixer1AlarmCountQuery();
        CQIAlarmCountQuery();
        CQITop5AlarmCountQuery();
    }

    public void TotalAlarmCountQuery()
    {
        nodata = LogicObject.GetVariable("NoData1");
        IUANode myModelObject = Project.Current.Get("Model/LogixAIDashboard/LogixAIData");

        mystore = LogicObject.GetVariable("MyDatabase");
        Store myDbStore = InformationModel.Get<Store>(mystore.Value);

        string sqlQuery = $"SELECT COUNT(ActiveState_Id) AS AlarmActive FROM AlarmsEventLogger1 " +
            $"WHERE ActiveState_Id = 1";

        // Prepare SQL Query
        // Execute query and check result
        try
        {
            Object[,] ResultSet;
            String[] Header;
            myDbStore.Query(sqlQuery, out Header, out ResultSet);
            if (ResultSet.GetLength(0) < 1)
            {
                nodata.Value = true;
                Log.Error(LogicObject.BrowseName, "Input query returned less than one line");
                return;
            }
            nodata.Value = false;

            myModelObject.GetVariable("TotalAlarmCount").Value = Convert.ToDouble(ResultSet[0, 0]);

        }
        catch (Exception ex)
        {
            Log.Error(LogicObject.BrowseName, ex.Message);
            return;
        }
    }

    public void TotalAckedAlarmCountQuery()
    {
        nodata = LogicObject.GetVariable("NoData2");
        IUANode myModelObject = Project.Current.Get("Model/LogixAIDashboard/LogixAIData");

        mystore = LogicObject.GetVariable("MyDatabase");
        Store myDbStore = InformationModel.Get<Store>(mystore.Value);

        string sqlQuery = $"SELECT COUNT(AckedState_Id) AS AckedState FROM AlarmsEventLogger1 " +
            $"WHERE AckedState_Id = 1";

        // Prepare SQL Query
        // Execute query and check result
        try
        {
            Object[,] ResultSet;
            String[] Header;
            myDbStore.Query(sqlQuery, out Header, out ResultSet);
            if (ResultSet.GetLength(0) < 1)
            {
                nodata.Value = true;
                Log.Error(LogicObject.BrowseName, "Input query returned less than one line");
                return;
            }
            nodata.Value = false;

            myModelObject.GetVariable("TotalAckedAlarmCount").Value = Convert.ToDouble(ResultSet[0, 0]);

        }
        catch (Exception ex)
        {
            Log.Error(LogicObject.BrowseName, ex.Message);
            return;
        }
    }

    public void TotalConfirmedAlarmCountQuery()
    {
        nodata = LogicObject.GetVariable("NoData3");
        IUANode myModelObject = Project.Current.Get("Model/LogixAIDashboard/LogixAIData");

        mystore = LogicObject.GetVariable("MyDatabase");
        Store myDbStore = InformationModel.Get<Store>(mystore.Value);

        string sqlQuery = $"SELECT COUNT(ConfirmedState_Id) AS ConfirmedState FROM AlarmsEventLogger1 " +
            $"WHERE ConfirmedState_Id = 1 AND AckedState_Id = 1";

        // Prepare SQL Query
        // Execute query and check result
        try
        {
            Object[,] ResultSet;
            String[] Header;
            myDbStore.Query(sqlQuery, out Header, out ResultSet);
            if (ResultSet.GetLength(0) < 1)
            {
                nodata.Value = true;
                Log.Error(LogicObject.BrowseName, "Input query returned less than one line");
                return;
            }
            nodata.Value = false;

            myModelObject.GetVariable("TotalConfirmedAlarmCount").Value = Convert.ToDouble(ResultSet[0, 0]);

        }
        catch (Exception ex)
        {
            Log.Error(LogicObject.BrowseName, ex.Message);
            return;
        }
    }

    public void Mixer1AlarmCountQuery()
    {
        nodata = LogicObject.GetVariable("NoData4");
        IUANode myModelObject = Project.Current.Get("Model/LogixAIDashboard/LogixAIData");

        mystore = LogicObject.GetVariable("MyDatabase");
        Store myDbStore = InformationModel.Get<Store>(mystore.Value);

        string sqlQuery = $"SELECT COUNT(ActiveState_Id) AS AlarmActive FROM AlarmsEventLogger1 " +
            $"WHERE ActiveState_Id = 1 AND ConditionName = 'Mixer01 Anormaly Alarm'";

        // Prepare SQL Query
        // Execute query and check result
        try
        {
            Object[,] ResultSet;
            String[] Header;
            myDbStore.Query(sqlQuery, out Header, out ResultSet);
            if (ResultSet.GetLength(0) < 1)
            {
                nodata.Value = true;
                Log.Error(LogicObject.BrowseName, "Input query returned less than one line");
                return;
            }
            nodata.Value = false;

            myModelObject.GetVariable("Mixer01PowerAlarmCount").Value = Convert.ToDouble(ResultSet[0, 0]);

        }
        catch (Exception ex)
        {
            Log.Error(LogicObject.BrowseName, ex.Message);
            return;
        }
    }

    public void CQIAlarmCountQuery()
    {
        nodata = LogicObject.GetVariable("NoData5");
        IUANode myModelObject = Project.Current.Get("Model/LogixAIDashboard/LogixAIData");

        mystore = LogicObject.GetVariable("MyDatabase");
        Store myDbStore = InformationModel.Get<Store>(mystore.Value);

        string sqlQuery = $"SELECT COUNT(ActiveState_Id) AS AlarmActive FROM AlarmsEventLogger1 " +
            $"WHERE ActiveState_Id = 1 AND SourceName = 'Val_CaseQuality'";

        // Prepare SQL Query
        // Execute query and check result
        try
        {
            Object[,] ResultSet;
            String[] Header;
            myDbStore.Query(sqlQuery, out Header, out ResultSet);
            if (ResultSet.GetLength(0) < 1)
            {
                nodata.Value = true;
                Log.Error(LogicObject.BrowseName, "Input query returned less than one line");
                return;
            }
            nodata.Value = false;

            myModelObject.GetVariable("CQIAlarmCount").Value = Convert.ToDouble(ResultSet[0, 0]);

        }
        catch (Exception ex)
        {
            Log.Error(LogicObject.BrowseName, ex.Message);
            return;
        }
    }

    public void CQITop5AlarmCountQuery()
    {
        nodata = LogicObject.GetVariable("NoData6");
        IUANode myModelObject = Project.Current.Get("Model/LogixAIDashboard/LogixAIData");

        mystore = LogicObject.GetVariable("MyDatabase");
        Store myDbStore = InformationModel.Get<Store>(mystore.Value);

        string sqlQuery = $"SELECT COUNT(ActiveState_Id) AS AlarmActive, ConditionName FROM AlarmsEventLogger1 " +
            $"WHERE ActiveState_Id = 1 AND SourceName = 'Val_CaseQuality' "+
            $"GROUP BY ConditionName ORDER BY AlarmActive DESC";

        // Prepare SQL Query
        // Execute query and check result
        try
        {
            Object[,] ResultSet;
            String[] Header;
            myDbStore.Query(sqlQuery, out Header, out ResultSet);
            if (ResultSet.GetLength(0) < 1)
            {
                nodata.Value = true;
                Log.Error(LogicObject.BrowseName, "Input query returned less than one line");
                return;
            }
            nodata.Value = false;

            for (int i = 0; i < ResultSet.GetLength(0); i++)
            {
                myModelObject.GetVariable("CQIAlarmCount" + (i + 1)).Value = Convert.ToDouble(ResultSet[i, 0]);
                myModelObject.GetVariable("CQIAlarmDesc" + (i + 1)).Value = Convert.ToString(ResultSet[i, 1]);

                if (i >= 4) break;
            }

        }
        catch (Exception ex)
        {
            Log.Error(LogicObject.BrowseName, ex.Message);
            return;
        }
    }

    private IUAVariable nodata;
    private IUAVariable mystore;

    private PeriodicTask myPeriodicTask;
}
