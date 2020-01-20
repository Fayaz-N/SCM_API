using DALayer.Emails;
using SCMModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DALayer.MPR
{
    public class MPRDA : IMPRDA
    {
        private IEmailTemplateDA emailTemplateDA = default(IEmailTemplateDA);
        public MPRDA(IEmailTemplateDA EmailTemplateDA)
        {
            this.emailTemplateDA = EmailTemplateDA;
        }
        YSCMEntities DB = new YSCMEntities();

        public DataTable getDBMastersList(DynamicSearchResult Result)
        {
            Result.connectionString = DB.Database.Connection.ConnectionString;
            DataTable dtDBMastersList = new DataTable();
            string query = "";
            if (!string.IsNullOrEmpty(Result.tableName))
            {
                query = "select * from " + Result.tableName;
                if (Result.sortBy != null)
                {
                    query += " order by " + Result.sortBy;
                }
            }
            else if (Result.query != "")
            {
                query = Result.query;
            }

            SqlConnection con = new SqlConnection(DB.Database.Connection.ConnectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtDBMastersList);
            con.Close();
            da.Dispose();

            return dtDBMastersList;
        }

        public bool addDataToDBMasters(DynamicSearchResult Result)
        {
            string query = "insert into " + Result.tableName + "(" + Result.columnNames + ")values('" + Result.columnValues + "')";

            SqlConnection con = new SqlConnection(DB.Database.Connection.ConnectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return true;
        }

        public bool updateDataToDBMasters(DynamicSearchResult Result)
        {
            Result.connectionString = DB.Database.Connection.ConnectionString;
            string query = Result.query;

            SqlConnection con = new SqlConnection(DB.Database.Connection.ConnectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return true;
        }
        public DataTable GetListItems(DynamicSearchResult Result)
        {
            Result.connectionString = DB.Database.Connection.ConnectionString;
            DataTable dataTable = new DataTable();
            string query = "select * from " + Result.tableName + " where" + " " + Result.searchCondition + "";

            SqlConnection con = new SqlConnection(Result.connectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dataTable);
            con.Close();
            da.Dispose();
            return dataTable;
        }

        public MPRRevision updateMPR(MPRRevision mpr)
        {
            MPRRevision mprRevisionDetails = new MPRRevision();
            if (mpr != null)
            {
                try
                {

                    DB.Configuration.ProxyCreationEnabled = false;
                    MPRDetail mprDetails = new MPRDetail();
                    List<MPRRevision> MPRRevisionResult = new List<MPRRevision>();
                    int requestionId = mpr.RequisitionId;
                    //mpr.RevisionId = 0;
                    //mpr.RequisitionId = 224;
                    //if (mpr.RevisionId == 0 && mpr.RequisitionId != 0 && !string.IsNullOrEmpty(mpr.MPRDetail.DocumentNo))
                    if (mpr.RevisionId == 0 && mpr.RequisitionId != 0)
                    {
                        MPRRevision mprLastRecord = DB.MPRRevisions.OrderByDescending(p => p.RevisionId).Where(li => li.RequisitionId == mpr.RequisitionId).FirstOrDefault<MPRRevision>();
                        mprLastRecord.BoolValidRevision = false;
                        DB.SaveChanges();
                        mprRevisionDetails = mpr;
                        mprRevisionDetails.RevisionNo = Convert.ToByte(mprLastRecord.RevisionNo + 1);
                        mprRevisionDetails.BoolValidRevision = true;
                        mprRevisionDetails.MPRDetail = null;
                        mprRevisionDetails.ApprovalStatus = mprRevisionDetails.CheckStatus = mprRevisionDetails.SecondApproversStatus = mprRevisionDetails.ThirdApproverStatus = "Pending";
                        DB.MPRRevisions.Add(mprRevisionDetails);
                        DB.SaveChanges();
                        requestionId = mprDetails.RequisitionId;
                    }
                    mprRevisionDetails = DB.MPRRevisions.Where(li => li.RevisionId == mpr.RevisionId && li.RequisitionId == mpr.RequisitionId).FirstOrDefault<MPRRevision>();
                    if (mprRevisionDetails == null)
                    {
                        Int64 sequenceNo = Convert.ToInt64(DB.MPRDetails.Max(li => li.MPRSeqNo));
                        if (sequenceNo == null || sequenceNo == 0)
                            sequenceNo = 1;
                        else
                        {
                            sequenceNo = sequenceNo + 1;
                        }
                        var value = DB.SP_sequenceNumber(sequenceNo).FirstOrDefault();

                        mpr.MPRDetail.DocumentNo = "MPR/" + DateTime.Now.ToString("MMyy") + "/" + value;
                        mpr.MPRDetail.MPRSeqNo = sequenceNo;
                        mpr.MPRDetail.SubmittedBy = mpr.PreparedBy;
                        mpr.MPRDetail.SubmittedDate = DateTime.Now;
                        mprRevisionDetails = mpr;
                        mprRevisionDetails.RevisionNo = 0;
                        mprRevisionDetails.BoolValidRevision = true;
                        mprRevisionDetails.ApprovalStatus = mprRevisionDetails.CheckStatus = mprRevisionDetails.SecondApproversStatus = mprRevisionDetails.ThirdApproverStatus = "Pending";
                        DB.MPRRevisions.Add(mprRevisionDetails);
                        DB.SaveChanges();
                        requestionId = mprDetails.RequisitionId;
                    }
                    else

                    {
                        if (mpr.MPRItemInfoes.Count > 0)
                        {
                            MPRItemInfo mPRItemInfo = mpr.MPRItemInfoes.FirstOrDefault();
                            mprRevisionDetails.MPRItemInfoes = DB.MPRItemInfoes.Where(li => li.RevisionId == mprRevisionDetails.RevisionId && li.Itemdetailsid == mPRItemInfo.Itemdetailsid).ToList();
                            if (mprRevisionDetails.MPRItemInfoes.Count == 0)
                                mprRevisionDetails.MPRItemInfoes.Add(mPRItemInfo);
                            else
                            {
                                foreach (MPRItemInfo item in mprRevisionDetails.MPRItemInfoes)
                                {
                                    item.Itemid = mPRItemInfo.Itemid;
                                    if (mPRItemInfo.Itemid == "NewItem")
                                        item.Itemid = "0000";
                                    item.ItemDescription = mPRItemInfo.ItemDescription;
                                    item.Quantity = mPRItemInfo.Quantity;
                                    item.UnitId = mPRItemInfo.UnitId;
                                    item.SOLineItemNo = mPRItemInfo.SOLineItemNo;
                                    item.ReferenceDocNo = mPRItemInfo.ReferenceDocNo;
                                    item.MfgModelNo = mPRItemInfo.MfgModelNo;
                                    item.MfgPartNo = mPRItemInfo.MfgPartNo;
                                    item.TargetSpend = mPRItemInfo.TargetSpend;
                                }
                            }
                            DB.SaveChanges();
                        }

                        if (mpr.MPRDocuments.Count > 0)
                        {
                            foreach (MPRDocument item in mpr.MPRDocuments)
                            {
                                item.RevisionId = mprRevisionDetails.RevisionId;
                                //item.RevisionId = Convert.ToInt32(mprRevisionDetails.MPRItemInfoes.FirstOrDefault().RevisionId);
                                if (item.DocumentTypeid == 1)
                                    item.ItemDetailsId = mprRevisionDetails.MPRItemInfoes.FirstOrDefault().Itemdetailsid;
                                item.UploadedBy = mpr.PreparedBy; ;
                                item.UplaodedDate = DateTime.Now;
                                //item.Path = "";
                                item.Deleteflag = false;
                                if (item.MprDocId == 0)
                                    mprRevisionDetails.MPRDocuments.Add(item);
                                else
                                {
                                    MPRDocument MPRDocuments = DB.MPRDocuments.Where(li => li.RevisionId == mprRevisionDetails.RevisionId && li.ItemDetailsId == item.ItemDetailsId && li.MprDocId == item.MprDocId).FirstOrDefault();
                                    if (MPRDocuments != null)
                                    {
                                        MPRDocuments.DocumentName = item.DocumentName;
                                        DB.SaveChanges();
                                    }
                                }


                            }
                            DB.SaveChanges();

                        }

                        else if (mpr.MPRVendorDetails.Count > 0)
                        {

                            List<MPRVendorDetail> vendorList = mpr.MPRVendorDetails.ToList();
                            updateMPRVendor(vendorList, mpr.RevisionId);
                        }

                        else if (mpr.MPRDocumentations.Count > 0)
                        {
                            foreach (MPRDocumentation item in mpr.MPRDocumentations)
                            {
                                item.RevisionId = mprRevisionDetails.RevisionId;
                                //item.RevisionId = Convert.ToInt32(mprRevisionDetails.MPRItemInfoes.FirstOrDefault().RevisionId);


                                if (item.DocumentationId == 0)
                                    mprRevisionDetails.MPRDocumentations.Add(item);
                                else
                                {
                                    MPRDocumentation MPRDocumentations = DB.MPRDocumentations.Where(li => li.RevisionId == mprRevisionDetails.RevisionId && li.DocumentationId == item.DocumentationId).FirstOrDefault();

                                    //MPRDocumentations.DocumentationId = item.DocumentationId;
                                    MPRDocumentations.DocumentationDescriptionId = item.DocumentationDescriptionId;
                                    MPRDocumentations.NoOfSetsApproval = item.NoOfSetsApproval;
                                    MPRDocumentations.NoOfSetsFinal = item.NoOfSetsFinal;
                                    DB.SaveChanges();
                                }

                            }
                            DB.SaveChanges();
                        }

                        else if (mpr.MPRIncharges.Count > 0)
                        {

                            foreach (MPRIncharge item in mpr.MPRIncharges)
                            {
                                item.UpdatedBy = mpr.PreparedBy;
                                item.UpdatedDate = DateTime.Now;
                                item.DeleteFlag = false;
                                item.RequisitionId = mpr.RequisitionId;
                                item.RevisionId = mpr.RevisionId;

                                if (item.InchargeId == 0)
                                    mprRevisionDetails.MPRIncharges.Add(item);
                                else
                                {
                                    MPRIncharge MPRIncharage = DB.MPRIncharges.Where(li => li.RevisionId == mprRevisionDetails.RevisionId && li.InchargeId == item.InchargeId).FirstOrDefault();
                                    if (MPRIncharage != null)
                                    {
                                        MPRIncharage.RequisitionId = item.RequisitionId;
                                        MPRIncharage.Incharge = item.Incharge;
                                        MPRIncharage.CanClearTechnically = item.CanClearTechnically;
                                        MPRIncharage.CanClearCommercially = item.CanClearCommercially;
                                        MPRIncharage.CanReceiveMailNotification = item.CanReceiveMailNotification;
                                        MPRIncharage.UpdatedBy = item.UpdatedBy;
                                        MPRIncharage.UpdatedDate = DateTime.Now;
                                        MPRIncharage.DeleteFlag = item.DeleteFlag;
                                    }
                                }
                            }
                            DB.SaveChanges();
                        }

                        else if (mpr.MPRCommunications.Count > 0)
                        {

                            foreach (MPRCommunication item in mpr.MPRCommunications)
                            {
                                List<MPRReminderTracking> trackingDetails = new List<MPRReminderTracking>();

                                if (item.MPRCCId == 0)
                                {

                                    trackingDetails = item.MPRReminderTrackings.ToList();
                                    item.MPRReminderTrackings = null;
                                    mprRevisionDetails.MPRCommunications.Add(item);
                                    DB.SaveChanges();
                                }
                                else
                                {
                                    MPRCommunication MPRcommunication = DB.MPRCommunications.Where(li => li.RevisionId == mprRevisionDetails.RevisionId && li.MPRCCId == item.MPRCCId).FirstOrDefault();
                                    if (MPRcommunication != null)
                                    {
                                        MPRcommunication.Remarks = item.Remarks;
                                        MPRcommunication.RemarksFrom = item.RemarksFrom;
                                        MPRcommunication.SendEmail = item.SendEmail;
                                        MPRcommunication.SetReminder = item.SetReminder;
                                        MPRcommunication.ReminderDate = item.ReminderDate;
                                        MPRcommunication.RemarksDate = item.RemarksDate;
                                        DB.SaveChanges();
                                    }
                                }
                                foreach (MPRReminderTracking trackItem in trackingDetails)
                                {
                                    trackItem.MPRCCId = item.MPRCCId;
                                    MPRReminderTracking MPRRemindertracking = DB.MPRReminderTrackings.Where(li => li.ReminderId == trackItem.ReminderId && li.MPRCCId == item.MPRCCId).FirstOrDefault();
                                    if (MPRRemindertracking == null)
                                    {
                                        DB.MPRReminderTrackings.Add(trackItem);
                                        DB.SaveChanges();
                                    }
                                    else
                                    {

                                        if (MPRRemindertracking != null)
                                        {
                                            MPRRemindertracking.MailTo = trackItem.MailTo;
                                            MPRRemindertracking.MailAddressType = trackItem.MailAddressType;
                                            MPRRemindertracking.ReminderSentOn = trackItem.ReminderSentOn;
                                            MPRRemindertracking.MailSentOn = trackItem.MailSentOn;
                                        }
                                        DB.SaveChanges();
                                    }
                                    this.emailTemplateDA.prepareEmailTemplate("", mprRevisionDetails.RevisionId, mprRevisionDetails.PreparedBy, trackItem.MailTo, item.Remarks);
                                }
                            }

                        }

                        mprRevisionDetails.MPRDetail = DB.MPRDetails.Where(li => li.RequisitionId == mprRevisionDetails.RequisitionId).FirstOrDefault<MPRDetail>();
                        mprRevisionDetails.MPRDetail.DocumentDescription = mpr.MPRDetail.DocumentDescription;
                        mprRevisionDetails.IssuePurposeId = mpr.IssuePurposeId;
                        mprRevisionDetails.DepartmentId = mpr.DepartmentId;
                        mprRevisionDetails.ProjectManager = mpr.ProjectManager;
                        mprRevisionDetails.JobName = mpr.JobName;
                        mprRevisionDetails.JobCode = mpr.JobCode;
                        mprRevisionDetails.GEPSApprovalId = mpr.GEPSApprovalId;
                        mprRevisionDetails.SaleOrderNo = mpr.SaleOrderNo;
                        mprRevisionDetails.ClientName = mpr.ClientName;
                        mprRevisionDetails.PlantLocation = mpr.PlantLocation;
                        mprRevisionDetails.BuyerGroupId = mpr.BuyerGroupId;

                        mprRevisionDetails.TargetedSpendRemarks = mpr.TargetedSpendRemarks;
                        mprRevisionDetails.PurchaseTypeId = mpr.PurchaseTypeId;
                        mprRevisionDetails.PreferredVendorTypeId = mpr.PreferredVendorTypeId;
                        mprRevisionDetails.JustificationForSinglePreferredVendor = mpr.JustificationForSinglePreferredVendor;
                        mprRevisionDetails.DeliveryRequiredBy = mpr.DeliveryRequiredBy;
                        mprRevisionDetails.DispatchLocation = mpr.DispatchLocation;
                        mprRevisionDetails.ScopeId = mpr.ScopeId;
                        mprRevisionDetails.TrainingRequired = mpr.TrainingRequired;
                        mprRevisionDetails.TrainingManWeeks = mpr.TrainingManWeeks;
                        mprRevisionDetails.TrainingRemarks = mpr.TrainingRemarks;
                        mprRevisionDetails.BoolDocumentationApplicable = mpr.BoolDocumentationApplicable;

                        mprRevisionDetails.GuaranteePeriod = mpr.GuaranteePeriod;
                        mprRevisionDetails.NoOfSetsOfQAP = mpr.NoOfSetsOfQAP;
                        mprRevisionDetails.InspectionRequired = mpr.InspectionRequired;
                        mprRevisionDetails.InspectionComments = mpr.InspectionComments;
                        mprRevisionDetails.NoOfSetsOfTestCertificates = mpr.NoOfSetsOfTestCertificates;
                        mprRevisionDetails.ProcurementSourceId = mpr.ProcurementSourceId;
                        mprRevisionDetails.CustomsDutyId = mpr.CustomsDutyId;
                        mprRevisionDetails.ProjectDutyApplicableId = mpr.ProjectDutyApplicableId;
                        mprRevisionDetails.Remarks = mpr.Remarks;
                        mprRevisionDetails.CheckedBy = mpr.CheckedBy;
                        if (!string.IsNullOrEmpty(mpr.CheckedBy))
                        {
                            MPRStatusTrack mPRStatusTrackDetails = new MPRStatusTrack();
                            mPRStatusTrackDetails.RequisitionId = mpr.RequisitionId;
                            mPRStatusTrackDetails.RevisionId = mpr.RevisionId;
                            mPRStatusTrackDetails.StatusId = 1;
                            mPRStatusTrackDetails.UpdatedBy = mpr.PreparedBy;
                            mPRStatusTrackDetails.UpdatedDate = DateTime.Now;
                            updateMprstatusTrack(mPRStatusTrackDetails);
                        }
                        mprRevisionDetails.ApprovedBy = mpr.ApprovedBy;
                        DB.SaveChanges();

                    }

                    if (mprRevisionDetails != null)
                    {
                        mprRevisionDetails = getMPRRevisionDetails(mprRevisionDetails.RevisionId);
                    }
                    return mprRevisionDetails;
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
            return mprRevisionDetails;
        }
        public int addNewVendor(VendorMaster vendor)
        {
            using (YSCMEntities Context = new YSCMEntities())
            {
                vendor.AutoAssignmentofRFQ = true;
                vendor.Deleteflag = true;
                Context.VendorMasters.Add(vendor);
                Context.SaveChanges();
                return vendor.Vendorid;
            }
        }
        public bool deleteMPRDocument(MPRDocument mprDocument)
        {
            using (YSCMEntities Context = new YSCMEntities())
            {
                MPRDocument deptDelete = Context.MPRDocuments.Find(mprDocument.MprDocId);
                Context.MPRDocuments.Remove(deptDelete);
                Context.SaveChanges();
            }
            return true;
        }

        public bool deleteMPRItemInfo(MPRItemInfo mprItemInfo)
        {
            using (YSCMEntities Context = new YSCMEntities())
            {
                MPRItemInfo deptDelete = Context.MPRItemInfoes.Find(mprItemInfo.Itemdetailsid);
                Context.MPRItemInfoes.Remove(deptDelete);
                Context.SaveChanges();
            }
            return true;
        }

        public bool deleteMPRVendor(MPRVendorDetail mprVendor)
        {
            using (YSCMEntities Context = new YSCMEntities())
            {
                MPRVendorDetail deptDelete = Context.MPRVendorDetails.Find(mprVendor.VendorDetailsId);
                Context.MPRVendorDetails.Remove(deptDelete);
                Context.SaveChanges();
            }
            return true;
        }

        public bool deleteMPRDocumentation(MPRDocumentation MPRDocumentation)
        {
            using (YSCMEntities Context = new YSCMEntities())
            {
                MPRDocumentation deptDelete = Context.MPRDocumentations.Find(MPRDocumentation.DocumentationId);
                Context.MPRDocumentations.Remove(deptDelete);
                Context.SaveChanges();
            }
            return true;
        }

        public MPRRevision getMPRRevisionDetails(int RevisionId)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            MPRRevision mprRevisionDetails = new MPRRevision();

            mprRevisionDetails = DB.MPRRevisions.Include(x => x.MPRDetail).Include(x => x.MPRDepartment).Include(x => x.MPRProcurementSource)
                 .Include(x => x.MPRCustomsDuty).Include(x => x.MPRProjectDutyApplicable).Include(x => x.MPRBuyerGroup).Include(x => x.MPRItemInfoes)
                 .Include(x => x.MPRDocuments).Include(x => x.MPRDocumentations).Include(x => x.MPRVendorDetails).Include(x => x.MPRIncharges).Include(x => x.MPRCommunications).Where(li => li.RevisionId == RevisionId).FirstOrDefault<MPRRevision>();

            //if (mprRevisionDetails != null)
            //{
            //	mprRevisionDetails.MPRDetail = DB.MPRDetails.Where(li => li.RequisitionId == mprRevisionDetails.RequisitionId).FirstOrDefault<MPRDetail>();
            //	mprRevisionDetails.MPRDepartment = DB.MPRDepartments.Where(li => li.DepartmentId == mprRevisionDetails.DepartmentId).FirstOrDefault<MPRDepartment>();


            //	mprRevisionDetails.MPRProcurementSource = DB.MPRProcurementSources.Where(li => li.ProcurementSourceId == mprRevisionDetails.ProcurementSourceId).FirstOrDefault<MPRProcurementSource>();
            //	mprRevisionDetails.MPRCustomsDuty = DB.MPRCustomsDuties.Where(li => li.CustomsDutyId == mprRevisionDetails.CustomsDutyId).FirstOrDefault<MPRCustomsDuty>();
            //	mprRevisionDetails.MPRProjectDutyApplicable = DB.MPRProjectDutyApplicables.Where(li => li.ProjectDutyApplicableId == mprRevisionDetails.ProjectDutyApplicableId).FirstOrDefault<MPRProjectDutyApplicable>();


            //	//mprRevisionDetails.MPRScope = DB.MPRScopes.Where(li => li.ScopeId == mprRevisionDetails.ScopeId).FirstOrDefault<MPRScope>();
            //	mprRevisionDetails.MPRBuyerGroup = DB.MPRBuyerGroups.Where(li => li.BuyerGroupId == mprRevisionDetails.BuyerGroupId).FirstOrDefault<MPRBuyerGroup>();
            //	mprRevisionDetails.MPRItemInfoes = DB.MPRItemInfoes.Where(li => li.RevisionId == mprRevisionDetails.RevisionId).ToList();
            //	mprRevisionDetails.MPRDocuments = DB.MPRDocuments.Where(li => li.RevisionId == mprRevisionDetails.RevisionId).ToList();
            //	mprRevisionDetails.MPRDocumentations = DB.MPRDocumentations.Where(li => li.RevisionId == mprRevisionDetails.RevisionId).ToList();
            //	mprRevisionDetails.MPRVendorDetails = DB.MPRVendorDetails.Where(li => li.RevisionId == mprRevisionDetails.RevisionId).ToList();
            //	mprRevisionDetails.MPRIncharges = DB.MPRIncharges.Where(li => li.RevisionId == mprRevisionDetails.RevisionId).ToList();
            //	//mprRevisionDetails.MPRCommunications = DB.MPRCommunications.Include("MPRReminderTrackings").Where(li=>li.RevisionId==mprRevisionDetails.RevisionId).ToList();
            //	mprRevisionDetails.MPRCommunications = DB.MPRCommunications.Where(x => x.RevisionId == mprRevisionDetails.RevisionId).Include(x => x.MPRReminderTrackings).ToList<MPRCommunication>();

            //}
            foreach (MPRVendorDetail item in mprRevisionDetails.MPRVendorDetails)
            {
                item.VendorMaster = DB.VendorMasters.Where(li => li.Vendorid == item.Vendorid).FirstOrDefault();
            }
            foreach (MPRDocumentation item in mprRevisionDetails.MPRDocumentations)
            {
                item.MPRDocumentationDescription = DB.MPRDocumentationDescriptions.Where(li => li.DocumentationDescriptionId == item.DocumentationDescriptionId).FirstOrDefault();
            }
            foreach (MPRCommunication item in mprRevisionDetails.MPRCommunications)
            {
                item.Employee = DB.Employees.Where(li => li.EmployeeNo == item.RemarksFrom).FirstOrDefault();
                item.MPRReminderTrackings = DB.MPRReminderTrackings.Include(x => x.Employee).Where(li => li.MPRCCId == item.MPRCCId).ToList();

            }
            return mprRevisionDetails;


        }

        public List<MPRRevisionDetail> getMPRList(mprFilterParams mprfilterparams)
        {
            using (YSCMEntities Context = new YSCMEntities())
            {
                var query = default(string);
                //var frmDate = mprfilterparams.FromDate.ToString("yyyy-MM-dd");
                //var toDate = mprfilterparams.ToDate.ToString("yyyy-MM-dd");
                query = "Select * from MPRRevisionDetails Where BoolValidRevision='true' and PreparedOn <= '" + mprfilterparams.ToDate + "' and PreparedOn >= '" + mprfilterparams.FromDate + "'";
                //query = "Select * from MPRRevisionDetails Where BoolValidRevision='true' and PreparedOn <= " + mprfilterparams.ToDate.ToString() + " and PreparedOn >= " + mprfilterparams.FromDate.ToString() + "";
                if (!string.IsNullOrEmpty(mprfilterparams.PreparedBy))
                    query += " and PreparedBy = '" + mprfilterparams.PreparedBy + "'";
                if (mprfilterparams.ListType == "MPRPendingList")
                    query += " and CheckedBy ='-'";
                if (!string.IsNullOrEmpty(mprfilterparams.DocumentNo))
                    query += " and DocumentNo='" + mprfilterparams.DocumentNo + "'";
                if (!string.IsNullOrEmpty(mprfilterparams.DocumentDescription))
                    query += " and DocumentDescription='" + mprfilterparams.DocumentDescription + "'";
                if (!string.IsNullOrEmpty(mprfilterparams.CheckedBy))
                    query += " and CheckedBy=" + mprfilterparams.CheckedBy + " and CheckStatus='" + mprfilterparams.Status + "'";
                if (!string.IsNullOrEmpty(mprfilterparams.ApprovedBy))
                    query += " and ApprovedBy=" + mprfilterparams.ApprovedBy + " and ApprovalStatus='" + mprfilterparams.Status + "'";

                if (!string.IsNullOrEmpty(mprfilterparams.DepartmentId))
                    query += " and DepartmentId='" + mprfilterparams.DepartmentId + "'";
                if (!string.IsNullOrEmpty(mprfilterparams.JobCode))
                    query += " and JobCode='" + mprfilterparams.JobCode + "'";
                if (!string.IsNullOrEmpty(mprfilterparams.IssuePurposeId))
                    query += " and IssuePurposeId='" + mprfilterparams.IssuePurposeId + "'";
                if (!string.IsNullOrEmpty(mprfilterparams.ItemDescription))
                    query += " and ItemDescription='" + mprfilterparams.ItemDescription + "'";
                if (!string.IsNullOrEmpty(mprfilterparams.GEPSApprovalId))
                    query += " and GEPSApprovalId='" + mprfilterparams.JobCode + "'";
                if (!string.IsNullOrEmpty(mprfilterparams.BuyerGroupId))
                    query += " and BuyerGroupId='" + mprfilterparams.BuyerGroupId + "'";


                //if (!string.IsNullOrEmpty(mprfilterparams.CheckedBy))
                //	mprRevisionDetails = DB.MPRRevisionDetails.Where(li => li.BoolValidRevision == true && (li.PreparedOn <= mprfilterparams.ToDate && li.PreparedOn >= mprfilterparams.FromDate) && (li.CheckedBy == mprfilterparams.CheckedBy) && (li.CheckStatus == mprfilterparams.Status)).OrderBy(li => li.PreparedOn).ToList();
                //else if (!string.IsNullOrEmpty(mprfilterparams.ApprovedBy))
                //	mprRevisionDetails = DB.MPRRevisionDetails.Where(li => li.BoolValidRevision == true && (li.PreparedOn <= mprfilterparams.ToDate && li.PreparedOn >= mprfilterparams.FromDate) && (li.ApprovedBy == mprfilterparams.ApprovedBy) && (li.ApprovalStatus == mprfilterparams.Status)).OrderBy(li => li.PreparedOn).ToList();
                //else
                //	mprRevisionDetails = DB.MPRRevisionDetails.Where(li => li.BoolValidRevision == true && (li.PreparedOn <= mprfilterparams.ToDate && li.PreparedOn >= mprfilterparams.FromDate)).OrderBy(li => li.PreparedOn).ToList();
                //mprRevisionDetails.ForEach(a => a.MPRDetail = DB.MPRDetails.Where(li => li.RequisitionId == a.RequisitionId).FirstOrDefault());
                return Context.Database.SqlQuery<MPRRevisionDetail>(query).ToList();
            }

        }

        public int getMPRPendingListCnt(string PreparedBy)
        {
            return DB.MPRRevisionDetails.Where(li => li.BoolValidRevision == true && li.PreparedBy == PreparedBy && li.CheckedBy == "-").Count();
        }
        public List<Employee> getEmployeeList()
        {
            DB.Configuration.ProxyCreationEnabled = false;
            return DB.Employees.ToList();
        }

        public List<MPRRevisionDetail> getMPRRevisionList(int RequisitionId)
        {
            DB.Configuration.ProxyCreationEnabled = false;
            List<MPRRevisionDetail> mprRevisionDetails;
            mprRevisionDetails = DB.MPRRevisionDetails.Where(li => li.RequisitionId == RequisitionId).ToList();
            foreach (MPRRevisionDetail item in mprRevisionDetails)
            {
                item.MPRStatusTrackDetails = DB.MPRStatusTrackDetails.Where(li => li.RequisitionId == RequisitionId).ToList();
            }

            //mprRevisionDetails.ForEach(a => a.MPRDetail = DB.MPRDetails.Where(li => li.RequisitionId == a.RequisitionId).FirstOrDefault());
            return mprRevisionDetails;
        }

        public MPRRevision statusUpdate(MPRStatusUpdate mprStatus)
        {
            MPRRevision mprrevision = new MPRRevision();
            try
            {
                MPRStatusTrack mPRStatusTrackDetails = new MPRStatusTrack();
                mPRStatusTrackDetails.RequisitionId = mprStatus.RequisitionId;
                mPRStatusTrackDetails.RevisionId = mprStatus.RevisionId;
                mPRStatusTrackDetails.UpdatedBy = mprStatus.PreparedBy;
                mPRStatusTrackDetails.UpdatedDate = DateTime.Now;
                using (YSCMEntities Context = new YSCMEntities())
                {
                    mprrevision = Context.MPRRevisions.Find(mprStatus.RevisionId);
                    if (mprStatus.typeOfuser == "Acknowledge")
                    {
                        mPRStatusTrackDetails.StatusId = 4;
                        updateMprstatusTrack(mPRStatusTrackDetails);
                    }
                    else
                    {
                        if (mprStatus.typeOfuser == "Checker")
                        {
                            mprrevision.CheckStatus = mprStatus.status;
                            mprrevision.CheckerRemarks = mprStatus.Remarks;
                            mprrevision.CheckedOn = DateTime.Now;
                            mPRStatusTrackDetails.StatusId = 2;
                        }
                        else if (mprStatus.typeOfuser == "Approver")
                        {
                            mprrevision.ApprovalStatus = mprStatus.status;
                            mprrevision.ApproverRemarks = mprStatus.Remarks;
                            mprrevision.ApprovedOn = DateTime.Now;
                            mPRStatusTrackDetails.StatusId = 3;
                        }
                        else if (mprStatus.typeOfuser == "SecondApprover")
                        {
                            mprrevision.SecondApproversStatus = mprStatus.status;
                            mprrevision.SecondApproverRemarks = mprStatus.Remarks;
                            mprrevision.SecondApprovedOn = DateTime.Now;
                        }
                        else if (mprStatus.typeOfuser == "ThirdApprover")
                        {
                            mprrevision.ThirdApproverStatus = mprStatus.status;
                            mprrevision.ThirdApproverRemarks = mprStatus.Remarks;
                            mprrevision.ThirdApproverStatusChangedOn = DateTime.Now;
                        }

                        Context.SaveChanges();
                        if (mprStatus.status == "Approved")
                            updateMprstatusTrack(mPRStatusTrackDetails);
                    }
                    this.emailTemplateDA.prepareEmailTemplate(mprStatus.typeOfuser, mprStatus.RevisionId, "", "", "");
                }

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            return this.getMPRRevisionDetails(mprStatus.RevisionId);
        }
        public List<SCMStatu> getStatusList()
        {
            using (YSCMEntities Context = new YSCMEntities())
            {
                return Context.SCMStatus.ToList();
            }
        }

        public List<UserPermission> getAccessList(int RoleId)
        {
            using (YSCMEntities context = new YSCMEntities())
            {
                return context.UserPermissions.Where(li => li.RoleId == RoleId).ToList();
            }
        }
        public bool updateMPRVendor(List<MPRVendorDetail> MPRVendorDetails, int RevisionId)
        {
            foreach (MPRVendorDetail item in MPRVendorDetails)
            {
                item.RevisionId = RevisionId;
                //item.RevisionId = Convert.ToInt32(mprRevisionDetails.MPRItemInfoes.FirstOrDefault().RevisionId);

                item.UpdatedBy = item.UpdatedBy;
                item.UpdatedDate = DateTime.Now;
                item.RemoveFlag = false;
                if (item.VendorDetailsId == 0)
                    DB.MPRVendorDetails.Add(item);
                else
                {
                    MPRVendorDetail MPRVendordetails = DB.MPRVendorDetails.Where(li => li.RevisionId == RevisionId && li.VendorDetailsId == item.VendorDetailsId).FirstOrDefault();

                    MPRVendordetails.Vendorid = item.Vendorid;

                }
                DB.SaveChanges();
            }
            List<MPRVendorDetail> mprvendorDetails = DB.MPRVendorDetails.Where(li => li.RevisionId == RevisionId).ToList();
            return true;
        }

        public void updateMprstatusTrack(MPRStatusTrack mprStatusTrackDetails)
        {
            if (mprStatusTrackDetails.StatusTrackId == 0)
                DB.MPRStatusTracks.Add(mprStatusTrackDetails);
            else
            {
                MPRStatusTrack mprStatusTrackDetailss = DB.MPRStatusTracks.Where(li => li.StatusTrackId == mprStatusTrackDetails.StatusTrackId).FirstOrDefault();

                mprStatusTrackDetailss.RequisitionId = mprStatusTrackDetailss.RequisitionId;
                mprStatusTrackDetailss.RevisionId = mprStatusTrackDetailss.RevisionId;
                mprStatusTrackDetailss.StatusId = mprStatusTrackDetailss.StatusId;
                mprStatusTrackDetailss.Remarks = mprStatusTrackDetailss.Remarks;
                mprStatusTrackDetailss.UpdatedBy = mprStatusTrackDetailss.UpdatedBy;
                mprStatusTrackDetailss.UpdatedDate = DateTime.Now;
            }
            DB.SaveChanges();
        }
    }
}
