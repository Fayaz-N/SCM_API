/*
    Name of File : <<MPRDA>>  Author :<<Prasanna>>  
    Date of Creation <<15-09-2019>>
    Purpose : <<This is Data access Layer to create MPR, get MPR data, MPR List>>
    Review Date :<<>>   Reviewed By :<<>>
    Version : 0.1 <change version only if there is major change - new release etc>
    Sourcecode Copyright : Yokogawa India Limited
*/
using DALayer.Common;
using DALayer.Emails;
using SCMModels;
using SCMModels.RemoteModel;
using SCMModels.RFQModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DALayer.MPR
{
	/*
Name of Class : <<MPRDA>>  Author :<<Prasanna>>  
Date of Creation <<15-09-2019>>
Purpose : <<to create MPR, get MPR data>>
Review Date :<<>>   Reviewed By :<<>>

*/
	public class MPRDA : IMPRDA
	{
		private IEmailTemplateDA emailTemplateDA = default(IEmailTemplateDA);
		private ErrorLog log = new ErrorLog();
		public MPRDA(IEmailTemplateDA EmailTemplateDA)
		{
			this.emailTemplateDA = EmailTemplateDA;
		}
		YSCMEntities DB = new YSCMEntities();

		/*Name of Function : <<getDBMastersList>>  Author :<<Prasanna>>  
	    Date of Creation <<15-09-2019>>
	    Purpose : <<get table data dynamically by passing query as parameter>>
	    Review Date :<<>>   Reviewed By :<<>>*/
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

		/*Name of Function : <<addDataToDBMasters>>  Author :<<Prasanna>>  
		Date of Creation <<15-09-2019>>
		Purpose : <<insert table data dynamically by passing query as parameter>>
		Review Date :<<>>   Reviewed By :<<>>*/
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


		/*Name of Function : <<updateDataToDBMasters>>  Author :<<Prasanna>>  
		Date of Creation <<15-09-2019>>
		Purpose : <<update table data dynamically by passing query as parameter>>
		Review Date :<<>>   Reviewed By :<<>>*/
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
		/*Name of Function : <<GetListItems>>  Author :<<Prasanna>>  
		Date of Creation <<15-09-2019>>
		Purpose : <<Get table data as list using dynamic query>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public DataTable GetListItems(DynamicSearchResult Result)
		{
			Result.connectionString = DB.Database.Connection.ConnectionString;
			DataTable dataTable = new DataTable();
			string query = "";
			query = "select * from " + Result.tableName + Result.searchCondition + "";
			if (!string.IsNullOrEmpty(Result.query))
				query = Result.query;
			SqlConnection con = new SqlConnection(Result.connectionString);
			SqlCommand cmd = new SqlCommand(query, con);
			con.Open();
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			da.Fill(dataTable);
			con.Close();
			da.Dispose();
			return dataTable;
		}

		/*Name of Function : <<updateMPR>>  Author :<<Prasanna>>  
		Date of Creation <<20-09-2019>>
		Purpose : <<insert,update mpr data>>
		Review Date :<<>>   Reviewed By :<<>>*/
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
						mprRevisionDetails.DeleteFlag = false;
						mprRevisionDetails.ApprovalStatus = mprRevisionDetails.CheckStatus = mprRevisionDetails.SecondApproversStatus = mprRevisionDetails.ThirdApproverStatus = mprRevisionDetails.OApprovalStatus = mprRevisionDetails.OCheckStatus = mprRevisionDetails.OSecondApproversStatus = mprRevisionDetails.OThirdApproverStatus = "Pending";
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
						mprRevisionDetails.DeleteFlag = false;
						mprRevisionDetails.ApprovalStatus = mprRevisionDetails.CheckStatus = mprRevisionDetails.SecondApproversStatus = mprRevisionDetails.ThirdApproverStatus = mprRevisionDetails.OApprovalStatus = mprRevisionDetails.OCheckStatus = mprRevisionDetails.OSecondApproversStatus = mprRevisionDetails.OThirdApproverStatus = "Pending";
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
							{
								if (string.IsNullOrEmpty(mPRItemInfo.Itemid))
									mPRItemInfo.Itemid = "NewItem";
								mPRItemInfo.Itemid = mPRItemInfo.Itemid;
								if (mPRItemInfo.Itemid == "NewItem" || mPRItemInfo.Itemid == "0000")
									mPRItemInfo.Itemid = "NewItem";
								mprRevisionDetails.MPRItemInfoes.Add(mPRItemInfo);
							}
							else
							{
								foreach (MPRItemInfo item in mprRevisionDetails.MPRItemInfoes)
								{
									if (string.IsNullOrEmpty(mPRItemInfo.Itemid))
										mPRItemInfo.Itemid = "NewItem";
									item.Itemid = mPRItemInfo.Itemid;
									if (mPRItemInfo.Itemid == "NewItem" || mPRItemInfo.Itemid == "0000")
										item.Itemid = "NewItem";
									item.ItemDescription = mPRItemInfo.ItemDescription;
									item.Quantity = mPRItemInfo.Quantity;
									item.UnitId = mPRItemInfo.UnitId;
									item.SOLineItemNo = mPRItemInfo.SOLineItemNo;
									item.ReferenceDocNo = mPRItemInfo.ReferenceDocNo;
									item.MfgModelNo = mPRItemInfo.MfgModelNo;
									item.MfgPartNo = mPRItemInfo.MfgPartNo;
									item.TargetSpend = mPRItemInfo.TargetSpend;
									item.RepeatOrderRefId = mPRItemInfo.RepeatOrderRefId;
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
									this.emailTemplateDA.prepareMPREmailTemplate("", mprRevisionDetails.RevisionId, mprRevisionDetails.PreparedBy, trackItem.MailTo, item.Remarks);
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
						mprRevisionDetails.InspectionRemarks = mpr.InspectionRemarks;
						mprRevisionDetails.NoOfSetsOfTestCertificates = mpr.NoOfSetsOfTestCertificates;
						mprRevisionDetails.ProcurementSourceId = mpr.ProcurementSourceId;
						mprRevisionDetails.CustomsDutyId = mpr.CustomsDutyId;
						mprRevisionDetails.ProjectDutyApplicableId = mpr.ProjectDutyApplicableId;
						mprRevisionDetails.Remarks = mpr.Remarks;
						mprRevisionDetails.MPRForOrdering = true;
						mprRevisionDetails.ORemarks = mpr.ORemarks;
						mprRevisionDetails.ORequestedBy = mpr.ORequestedBy;
						mprRevisionDetails.ORequestedon = DateTime.Now;

						//mprRevisionDetails.OCheckedBy = mpr.OCheckedBy;
						//mprRevisionDetails.OCheckedOn = DateTime.Now;

						//mprRevisionDetails.OApprovedBy = mpr.OApprovedBy;
						//mprRevisionDetails.OApprovedOn = DateTime.Now;

						//mprRevisionDetails.OSecondApprover = mpr.OSecondApprover;
						//mprRevisionDetails.OSecondApprovedOn = DateTime.Now;

						//mprRevisionDetails.OThirdApprover = mpr.OThirdApprover;
						//mprRevisionDetails.OThirdApproverStatusChangedOn = DateTime.Now;
						mprRevisionDetails.CheckedBy = mpr.CheckedBy;
						mprRevisionDetails.ApprovedBy = mpr.ApprovedBy;
						int cnt = DB.MPRStatusTrackDetails.Where(li => li.RequisitionId == mpr.RequisitionId && li.RevisionId == mpr.RevisionId && li.StatusId == 1).Count();//checking mpr generated already or not 
						if (!string.IsNullOrEmpty(mpr.CheckedBy))
						{
							if (cnt == 0)
							{
								MPRStatusTrack mPRStatusTrackDetails = new MPRStatusTrack();
								mPRStatusTrackDetails.RequisitionId = mpr.RequisitionId;
								mPRStatusTrackDetails.RevisionId = mpr.RevisionId;
								mPRStatusTrackDetails.StatusId = 1;
								mPRStatusTrackDetails.UpdatedBy = mpr.PreparedBy;
								mPRStatusTrackDetails.UpdatedDate = DateTime.Now;
								updateMprstatusTrack(mPRStatusTrackDetails);
								mprRevisionDetails.StatusId = 1;
								DB.SaveChanges();
								this.emailTemplateDA.prepareMPREmailTemplate("Requestor", mpr.RevisionId, mpr.PreparedBy, mpr.CheckedBy, "");
							}
						}

						if (mprRevisionDetails.PurchaseTypeId == 1)//for single vendor we are updating second and third approvers from mprdepartment table
						{
							mprRevisionDetails.SecondApprover = DB.MPRDepartments.Where(li => li.DepartmentId == mprRevisionDetails.DepartmentId).FirstOrDefault().SecondApprover;
							mprRevisionDetails.ThirdApprover = DB.MPRDepartments.Where(li => li.DepartmentId == mprRevisionDetails.DepartmentId).FirstOrDefault().ThirdApprover;
						}
						else
						{
							mprRevisionDetails.SecondApprover = null;
							mprRevisionDetails.ThirdApprover = null;
						}
						if (mpr.IssuePurposeId == 2)
						{
							mprRevisionDetails.MPRForOrdering = true;
							mprRevisionDetails.ORequestedBy = mprRevisionDetails.PreparedBy;
							mprRevisionDetails.ORequestedon = DateTime.Now;

							mprRevisionDetails.OCheckedBy = mprRevisionDetails.CheckedBy;
							//mprRevisionDetails.OCheckedOn = DateTime.Now;

							mprRevisionDetails.OApprovedBy = mprRevisionDetails.ApprovedBy;
							//mprRevisionDetails.OApprovedOn = DateTime.Now;

							mprRevisionDetails.OSecondApprover = mprRevisionDetails.SecondApprover;
							//mprRevisionDetails.OSecondApprovedOn = DateTime.Now;

							mprRevisionDetails.OThirdApprover = mprRevisionDetails.ThirdApprover;
							//mprRevisionDetails.OThirdApproverStatusChangedOn = DateTime.Now;
						}
						//update all statuses as pending from sent for modication
						if (mprRevisionDetails.CheckStatus == "Sent for Modification" || mprRevisionDetails.ApprovalStatus == "Sent for Modification" || mprRevisionDetails.SecondApproversStatus == "Sent for Modification" || mprRevisionDetails.ThirdApproverStatus == "Sent for Modification")
						{
							mprRevisionDetails.ApprovalStatus = mprRevisionDetails.CheckStatus = mprRevisionDetails.SecondApproversStatus = mprRevisionDetails.ThirdApproverStatus = mprRevisionDetails.OApprovalStatus = mprRevisionDetails.OCheckStatus = mprRevisionDetails.OSecondApproversStatus = mprRevisionDetails.OThirdApproverStatus = "Pending";
							DB.SaveChanges();
							this.emailTemplateDA.prepareMPREmailTemplate("Requestor", mpr.RevisionId, mpr.PreparedBy, mpr.CheckedBy, "");
						}
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
							log.ErrorMessage("MPRController", "updateMPR", ve.ErrorMessage);
							Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
								ve.PropertyName, ve.ErrorMessage);
						}
					}
					throw;
				}
			}
			return mprRevisionDetails;
		}

		/*Name of Function : <<copyMprRevision>>  Author :<<Prasanna>>  
		Date of Creation <<30-03-2020>>
		Purpose : <<copy existing MPR as new revision>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public MPRRevision copyMprRevision(MPRRevision mpr, bool repeatOrder, bool revise)
		{

			MPRRevision mprRevisionDetails = new MPRRevision();
			if (mpr != null)
			{
				try
				{
					int revisionId = mpr.RevisionId;
					int requisitinId = mpr.RequisitionId;
					DB.Configuration.ProxyCreationEnabled = false;

					List<MPRRevision> MPRRevisionResult = new List<MPRRevision>();
					Int64 sequenceNo = Convert.ToInt64(DB.MPRDetails.Max(li => li.MPRSeqNo));
					if (sequenceNo == null || sequenceNo == 0)
						sequenceNo = 1;
					else
					{
						sequenceNo = sequenceNo + 1;
					}
					var value = DB.SP_sequenceNumber(sequenceNo).FirstOrDefault();
					if (revise == true)
					{
						MPRRevision mprLastRecord = DB.MPRRevisions.OrderByDescending(p => p.RevisionId).Where(li => li.RequisitionId == mpr.RequisitionId).FirstOrDefault<MPRRevision>();
						mprLastRecord.BoolValidRevision = false;
						DB.SaveChanges();
						mprRevisionDetails = mpr;
						mprRevisionDetails.RequisitionId = mprLastRecord.RequisitionId;
						mprRevisionDetails.RevisionNo = Convert.ToByte(mprLastRecord.RevisionNo + 1);
						mprRevisionDetails.BoolValidRevision = true;
						mprRevisionDetails.MPRDetail = null;
						mprRevisionDetails.PreparedBy = mpr.PreparedBy;
						mprRevisionDetails.PreparedOn = DateTime.Now;
						mprRevisionDetails.DeleteFlag = false;
						mprRevisionDetails.ApprovalStatus = mprRevisionDetails.CheckStatus = mprRevisionDetails.SecondApproversStatus = mprRevisionDetails.ThirdApproverStatus = mprRevisionDetails.OApprovalStatus = mprRevisionDetails.OCheckStatus = mprRevisionDetails.OSecondApproversStatus = mprRevisionDetails.OThirdApproverStatus = "Pending";
						DB.MPRRevisions.Add(mprRevisionDetails);
						DB.SaveChanges();

					}
					else
					{
						MPRDetail MPRDetail = DB.MPRDetails.Where(li => li.RequisitionId == requisitinId).FirstOrDefault<MPRDetail>();
						mprRevisionDetails.MPRDetail = new MPRDetail();
						mprRevisionDetails.MPRDetail.DocumentNo = "MPR/" + DateTime.Now.ToString("MMyy") + "/" + value;
						mprRevisionDetails.MPRDetail.MPRSeqNo = sequenceNo;
						mprRevisionDetails.MPRDetail.SubmittedBy = mpr.PreparedBy;
						mprRevisionDetails.MPRDetail.SubmittedDate = DateTime.Now;
						mprRevisionDetails.MPRDetail.DocumentDescription = MPRDetail.DocumentDescription;
						mprRevisionDetails.RevisionNo = 0;
						mprRevisionDetails.BoolValidRevision = true;
						mprRevisionDetails.PreparedBy = mpr.PreparedBy;
						mprRevisionDetails.PreparedOn = DateTime.Now;
						mprRevisionDetails.DeleteFlag = false;
						mprRevisionDetails.ApprovalStatus = mprRevisionDetails.CheckStatus = mprRevisionDetails.SecondApproversStatus = mprRevisionDetails.ThirdApproverStatus = mprRevisionDetails.OApprovalStatus = mprRevisionDetails.OCheckStatus = mprRevisionDetails.OSecondApproversStatus = mprRevisionDetails.OThirdApproverStatus = "Pending";
						DB.MPRRevisions.Add(mprRevisionDetails);
						DB.SaveChanges();
					}
					if (repeatOrder == false)
						mpr = DB.MPRRevisions.Where(li => li.RevisionId == revisionId).FirstOrDefault();

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
					mprRevisionDetails.InspectionRemarks = mpr.InspectionRemarks;
					mprRevisionDetails.NoOfSetsOfTestCertificates = mpr.NoOfSetsOfTestCertificates;
					mprRevisionDetails.ProcurementSourceId = mpr.ProcurementSourceId;
					mprRevisionDetails.CustomsDutyId = mpr.CustomsDutyId;
					mprRevisionDetails.ProjectDutyApplicableId = mpr.ProjectDutyApplicableId;
					mprRevisionDetails.Remarks = mpr.Remarks;
					//mprRevisionDetails.CheckedBy = mpr.CheckedBy;

					DB.SaveChanges();
					List<MPRItemInfo> MPRItemInfoes = DB.MPRItemInfoes.Where(li => li.RevisionId == revisionId).ToList();
					if (repeatOrder == true)
						MPRItemInfoes = mpr.MPRItemInfoes.ToList();
					if (MPRItemInfoes.Count > 0)
					{

						foreach (MPRItemInfo mPRItemInfo in MPRItemInfoes)
						{
							MPRItemInfo item = new MPRItemInfo();
							item.Itemid = mPRItemInfo.Itemid;
							item.RevisionId = mprRevisionDetails.RevisionId;
							item.ItemDescription = mPRItemInfo.ItemDescription;
							item.Quantity = mPRItemInfo.Quantity;
							item.UnitId = mPRItemInfo.UnitId;
							item.SOLineItemNo = mPRItemInfo.SOLineItemNo;
							item.ReferenceDocNo = mPRItemInfo.ReferenceDocNo;
							item.MfgModelNo = mPRItemInfo.MfgModelNo;
							item.MfgPartNo = mPRItemInfo.MfgPartNo;
							item.TargetSpend = mPRItemInfo.TargetSpend;
							item.RepeatOrderRefId = mPRItemInfo.RepeatOrderRefId;
							DB.MPRItemInfoes.Add(item);
							DB.SaveChanges();
						}

					}
					List<MPRDocument> mprdocuments = DB.MPRDocuments.Where(li => li.RevisionId == revisionId).ToList();
					if (mprdocuments.Count > 0)
					{
						foreach (MPRDocument mprdoc in mprdocuments)
						{
							MPRDocument item = new MPRDocument();
							item.RevisionId = mprRevisionDetails.RevisionId;
							item.ItemDetailsId = mprdoc.ItemDetailsId;
							item.DocumentName = mprdoc.DocumentName;
							item.Path = mprdoc.Path;
							item.UploadedBy = mpr.PreparedBy; ;
							item.UplaodedDate = DateTime.Now;
							item.DocumentTypeid = mprdoc.DocumentTypeid;
							item.Deleteflag = mprdoc.Deleteflag;
							DB.MPRDocuments.Add(item);
							DB.SaveChanges();
						}


					}
					List<MPRVendorDetail> MPRVendorDetails = DB.MPRVendorDetails.Where(li => li.RevisionId == revisionId).ToList();

					if (MPRVendorDetails.Count > 0)
					{

						foreach (MPRVendorDetail MPRVendorDetail in MPRVendorDetails)
						{
							MPRVendorDetail item = new MPRVendorDetail();
							item.RevisionId = mprRevisionDetails.RevisionId;
							item.Vendorid = MPRVendorDetail.Vendorid;
							item.UpdatedBy = mpr.PreparedBy;
							item.UpdatedDate = DateTime.Now;
							DB.MPRVendorDetails.Add(item);
							DB.SaveChanges();
						}

					}
					List<MPRDocumentation> MPRDocumentations = DB.MPRDocumentations.Where(li => li.RevisionId == revisionId).ToList();

					if (MPRDocumentations.Count > 0)
					{
						foreach (MPRDocumentation mprdocment in MPRDocumentations)
						{
							MPRDocumentation item = new MPRDocumentation();
							item.RevisionId = mprRevisionDetails.RevisionId;
							item.DocumentationDescriptionId = mprdocment.DocumentationDescriptionId;
							item.NoOfSetsApproval = mprdocment.NoOfSetsApproval;
							item.NoOfSetsFinal = mprdocment.NoOfSetsFinal;
							DB.MPRDocumentations.Add(item);
							DB.SaveChanges();
						}

					}
					List<MPRIncharge> MPRIncharges = DB.MPRIncharges.Where(li => li.RevisionId == revisionId).ToList();

					if (MPRIncharges.Count > 0)
					{
						foreach (MPRIncharge mprincharge in MPRIncharges)
						{
							MPRIncharge item = new MPRIncharge();
							item.RequisitionId = mpr.RequisitionId;
							item.RevisionId = mpr.RevisionId;
							item.Incharge = mprincharge.Incharge;
							item.CanClearTechnically = mprincharge.CanClearTechnically;
							item.CanClearCommercially = mprincharge.CanClearCommercially;
							item.CanReceiveMailNotification = mprincharge.CanReceiveMailNotification;
							item.UpdatedBy = mpr.PreparedBy;
							item.UpdatedDate = DateTime.Now;
							item.DeleteFlag = false;

							DB.MPRIncharges.Add(item);
							DB.SaveChanges();
						}

					}

					if (mprRevisionDetails != null)
					{
						mprRevisionDetails = getMPRRevisionDetails(mprRevisionDetails.RevisionId);
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
							log.ErrorMessage("MPRController", "copyMprRevision", ve.ErrorMessage);
							Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
								ve.PropertyName, ve.ErrorMessage);
						}
					}
					throw;
				}
			}
			return mprRevisionDetails;
		}
		//public int addNewVendor(VendorMaster vendor)
		//{
		//    using (YSCMEntities Context = new YSCMEntities())
		//    {
		//        if (vendor.Vendorid == 0 || vendor.Vendorid == null)
		//        {
		//            vendor.AutoAssignmentofRFQ = true;
		//            vendor.Deleteflag = true;
		//            Context.VendorMasters.Add(vendor);
		//        }
		//        else
		//        {
		//            VendorMaster vendormaster = Context.VendorMasters.Where(li => li.Vendorid == vendor.Vendorid).FirstOrDefault();

		//            vendormaster.Emailid = vendor.Emailid;
		//        }
		//        Context.SaveChanges();
		//        return vendor.Vendorid;
		//    }
		//}

		/*Name of Function : <<addNewVendor>>  Author :<<Prasanna>>  
		Date of Creation <<30-03-2020>>
		Purpose : <<add new vendor in both vscm and yscm>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public int addNewVendor(VendormasterModel model)
		{
			int vendorid = model.Vendorid;
			VSCMEntities vscm = new VSCMEntities();
			RemoteVendorMaster vendor = new RemoteVendorMaster();
			if (model.Vendorid == 0 || model.Vendorid == null)
			{
				vendor.AutoAssignmentofRFQ = true;
				vendor.Deleteflag = true;
				vendor.VendorCode = model.VendorCode;
				vendor.VendorName = model.VendorName;
				vendor.OldVendorCode = model.OldVendorCode;
				vendor.Street = model.Street;
				vendor.City = model.City;
				//vendor.RegionCode = model.RegionCode;
				vendor.PostalCode = model.PostalCode;
				vendor.PhoneNo = model.ContactNumber;
				vendor.FaxNo = null;
				vendor.AuGr = null;
				vendor.PaymentTermCode = null;
				vendor.Blocked = null;
				vendor.AutoAssignmentofRFQ = true;
				vendor.Emailid = model.Emailid;
				vendor.Deleteflag = true;
				vscm.RemoteVendorMasters.Add(vendor);
				vscm.SaveChanges();
				vendorid = vendor.Vendorid;
			}
			else
			{
				var vendordata = vscm.RemoteVendorMasters.Where(x => x.Vendorid == model.Vendorid).FirstOrDefault();
				vendordata.VendorCode = vendordata.VendorCode;
				vendordata.VendorName = vendordata.VendorName;
				vendordata.OldVendorCode = vendordata.OldVendorCode;
				vendordata.Street = vendordata.Street;
				vendordata.City = vendordata.City;
				//vendordata.RegionCode = model.RegionCode;
				vendordata.PostalCode = vendordata.PostalCode;
				vendordata.PhoneNo = vendordata.ContactNo;
				vendordata.FaxNo = null;
				vendordata.AuGr = null;
				vendordata.PaymentTermCode = null;
				vendordata.Blocked = null;
				vendordata.AutoAssignmentofRFQ = vendordata.AutoAssignmentofRFQ;
				vendordata.Emailid = model.Emailid;
				vendordata.Deleteflag = true;
				vscm.SaveChanges();
				vendorid = vendordata.Vendorid;
			}
			List<string> EmailList = model.Emailid.Split(new char[] { ',' }).ToList();
			foreach (var item in EmailList)
			{
				Int32 sequenceNo = 0;
				string password = "";
				var value = "";
				RemoteVendorUserMaster vendorUsermaster = vscm.RemoteVendorUserMasters.Where(li => li.Vuserid == item).FirstOrDefault();

				//need to implement vUniqueId value
				if (vendorUsermaster == null && !string.IsNullOrEmpty(item))
				{
					RemoteVendorUserMaster vendorUsermasters = new RemoteVendorUserMaster();
					sequenceNo = Convert.ToInt32(vscm.RemoteVendorUserMasters.Max(li => li.SequenceNo));
					if (sequenceNo == null || sequenceNo == 0)
						sequenceNo = 1;
					else
					{
						sequenceNo = sequenceNo + 1;
					}
					value = DB.SP_sequenceNumber(sequenceNo).FirstOrDefault();
					vendorUsermasters.VuniqueId = "C" + value;
					vendorUsermasters.SequenceNo = sequenceNo;
					vendorUsermasters.Vuserid = item.Replace(" ", String.Empty);
					password = GeneratePassword();
					vendorUsermasters.pwd = password;
					vendorUsermasters.ContactNumber = model.ContactNumber;
					vendorUsermasters.ContactPerson = model.ContactPerson;
					vendorUsermasters.VendorId = vendorid;
					vendorUsermasters.Active = true;
					vendorUsermasters.SuperUser = true;
					vscm.RemoteVendorUserMasters.Add(vendorUsermasters);
					vscm.SaveChanges();
				}

				else
				{
					if (vendorUsermaster != null && !string.IsNullOrEmpty(item))
					{
						//vendorUsermaster.Vuserid = model.Emailid;
						// vendorUsermaster.pwd = GeneratePassword();
						//vendorUsermaster.VendorId = vendorid;
						vscm.SaveChanges();
					}

				}
				YSCMEntities Context1 = new YSCMEntities();
				VendorUserMaster venmaster = Context1.VendorUserMasters.Where(li => li.Vuserid == item).FirstOrDefault<VendorUserMaster>();
				if (venmaster == null && !string.IsNullOrEmpty(item))
				{
					VendorUserMaster vendorUsermasters = new VendorUserMaster();
					vendorUsermasters.Vuserid = item.Replace(" ", String.Empty);
					vendorUsermasters.pwd = password;
					vendorUsermasters.VendorId = vendorid;
					vendorUsermasters.ContactNumber = model.ContactNumber;
					vendorUsermasters.ContactPerson = model.ContactPerson;
					vendorUsermasters.Active = true;
					vendorUsermasters.SuperUser = true;
					vendorUsermasters.VuniqueId = "C" + value;
					vendorUsermasters.SequenceNo = sequenceNo;
					Context1.VendorUserMasters.Add(vendorUsermasters);
					Context1.SaveChanges();
				}

				else
				{
					// vendorUsermaster.Vuserid = model.Emailid;
					//venmaster.pwd = item.pwd;
					//vendorUsermaster.VendorId = vendorid;
					Context1.SaveChanges();

				}
			}

			using (YSCMEntities Context = new YSCMEntities())
			{
				VendorMaster localvendor = new VendorMaster();
				if (model.Vendorid == 0 || model.Vendorid == null)
				{
					localvendor.Vendorid = vendorid;
					localvendor.AutoAssignmentofRFQ = true;
					localvendor.Deleteflag = true;
					localvendor.VendorCode = model.VendorCode;
					localvendor.VendorName = model.VendorName;
					localvendor.OldVendorCode = model.OldVendorCode;
					localvendor.Street = model.Street;
					localvendor.City = model.City;
					//localvendor.RegionCode = model.RegionCode;
					localvendor.PostalCode = model.PostalCode;
					localvendor.PhoneNo = model.ContactNumber;
					//localvendor.FaxNo = model.FaxNo;
					//localvendor.AuGr = model.AuGr;
					//localvendor.PaymentTermCode = model.PaymentTermCode;
					//localvendor.Blocked = model.Blocked;
					localvendor.Emailid = model.Emailid;
					localvendor.AutoAssignmentofRFQ = true;
					localvendor.Deleteflag = true;
					Context.VendorMasters.Add(localvendor);
					Context.SaveChanges();
					vendorid = localvendor.Vendorid;
				}
				else
				{
					VendorMaster vendormaster = Context.VendorMasters.Where(li => li.Vendorid == vendorid).FirstOrDefault();
					vendormaster.VendorCode = vendormaster.VendorCode;
					vendormaster.VendorName = vendormaster.VendorName;
					vendormaster.OldVendorCode = vendormaster.OldVendorCode;
					vendormaster.Street = vendormaster.Street;
					vendormaster.City = vendormaster.City;
					//vendormaster.RegionCode = model.RegionCode;
					vendormaster.PostalCode = vendormaster.PostalCode;
					vendormaster.PhoneNo = model.ContactNumber;
					//vendormaster.FaxNo = model.FaxNo;
					//vendormaster.AuGr = model.AuGr;
					//vendormaster.PaymentTermCode = model.PaymentTermCode;
					//vendormaster.Blocked = model.Blocked;
					vendormaster.Emailid = model.Emailid;
					vendormaster.Deleteflag = true;
					vendormaster.AutoAssignmentofRFQ = true;
					Context.SaveChanges();
					vendorid = vendormaster.Vendorid;

				}
				//List<RemoteVendorUserMaster> remoteVendorUsermaster = vscm.RemoteVendorUserMasters.ToList();
				//foreach (var item in remoteVendorUsermaster)
				//{
				//	try
				//	{

				//		VendorUserMaster venmaster = Context.VendorUserMasters.Where(li => li.Vuserid == item.Vuserid).FirstOrDefault<VendorUserMaster>();
				//		if (venmaster == null)
				//		{
				//			VendorUserMaster vendorUsermasters = new VendorUserMaster();
				//			vendorUsermasters.Vuserid = item.Vuserid;
				//			vendorUsermasters.pwd = item.pwd;
				//			vendorUsermasters.VendorId = vendorid;
				//			vendorUsermasters.ContactNumber = item.ContactNumber;
				//			vendorUsermasters.ContactPerson = item.ContactPerson;
				//			vendorUsermasters.Active = true;
				//			vendorUsermasters.SuperUser = true;
				//			vendorUsermasters.VuniqueId = item.VuniqueId;
				//			vendorUsermasters.SequenceNo = item.SequenceNo;
				//			Context.VendorUserMasters.Add(vendorUsermasters);
				//			Context.SaveChanges();
				//		}

				//		else
				//		{
				//			// vendorUsermaster.Vuserid = model.Emailid;
				//			venmaster.pwd = item.pwd;
				//			//vendorUsermaster.VendorId = vendorid;
				//			Context.SaveChanges();

				//		}
				//	}
				//	catch (DbEntityValidationException e)
				//	{
				//		foreach (var eve in e.EntityValidationErrors)
				//		{
				//			Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
				//				eve.Entry.Entity.GetType().Name, eve.Entry.State);
				//			foreach (var ve in eve.ValidationErrors)
				//			{
				//				Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
				//					ve.PropertyName, ve.ErrorMessage);
				//			}
				//		}
				//	}
				//}
				return vendorid;
			}
		}

		/*Name of Function : <<deleteMPRDocument>>  Author :<<Prasanna>>  
		Date of Creation <<30-03-2020>>
		Purpose : <<to delete mpr documents>>
		Review Date :<<>>   Reviewed By :<<>>*/
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

		/*Name of Function : <<deleteMPRItemInfo>>  Author :<<Prasanna>>  
		Date of Creation <<30-03-2020>>
		Purpose : <<delete mpritem info>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public bool deleteMPRItemInfo(MPRItemInfo mprItemInfo)
		{
			using (YSCMEntities Context = new YSCMEntities())
			{
				foreach (var item in mprItemInfo.MPRDocuments)
				{
					MPRDocument docDel = Context.MPRDocuments.Find(item.MprDocId);
					Context.MPRDocuments.Remove(docDel);
					Context.SaveChanges();
				}

				MPRItemInfo deptDelete = Context.MPRItemInfoes.Find(mprItemInfo.Itemdetailsid);
				Context.MPRItemInfoes.Remove(deptDelete);
				Context.SaveChanges();
			}
			return true;
		}

		/*Name of Function : <<addMprItemInfo>>  Author :<<Prasanna>>  
		Date of Creation <<30-03-2020>>
		Purpose : <<add mpr item info>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public string addMprItemInfo(MPRItemInfo mPRItemInfo)
		{
			using (YSCMEntities context = new YSCMEntities())
			{
				try
				{
					MPRItemInfo item = new MPRItemInfo();
					item.Itemid = mPRItemInfo.Itemid;
					item.RevisionId = mPRItemInfo.RevisionId;
					item.ItemDescription = mPRItemInfo.ItemDescription;
					item.Quantity = mPRItemInfo.Quantity;
					item.UnitId = mPRItemInfo.UnitId;
					item.SOLineItemNo = mPRItemInfo.SOLineItemNo;
					item.ReferenceDocNo = mPRItemInfo.ReferenceDocNo;
					item.MfgModelNo = mPRItemInfo.MfgModelNo;
					item.MfgPartNo = mPRItemInfo.MfgPartNo;
					item.TargetSpend = mPRItemInfo.TargetSpend;
					item.RepeatOrderRefId = mPRItemInfo.RepeatOrderRefId;
					DB.MPRItemInfoes.Add(item);
					DB.SaveChanges();
					return "true";
				}
				catch (DbEntityValidationException e)
				{
					string errmsg = "";
					foreach (var eve in e.EntityValidationErrors)
					{
						Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
							eve.Entry.Entity.GetType().Name, eve.Entry.State);
						foreach (var ve in eve.ValidationErrors)
						{
							errmsg = ve.PropertyName + ve.ErrorMessage;
						}
					}
					log.ErrorMessage("MPRController", "addMprItemInfo", errmsg);
					return errmsg;


				}
			}
		}

		/*Name of Function : <<deleteMPRVendor>>  Author :<<Prasanna>>  
		Date of Creation <<30-03-2020>>
		Purpose : <<deleteMPRVendor>>
		Review Date :<<>>   Reviewed By :<<>>*/
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

		/*Name of Function : <<deleteMPRDocumentation>>  Author :<<Prasanna>>  
		Date of Creation <<02-01-2020>>
		Purpose : <<deleteMPRVendor>>
		Review Date :<<>>   Reviewed By :<<>>*/
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

		/*Name of Function : <<getMPRRevisionDetails>>  Author :<<Prasanna>>  
		Date of Creation <<06-12-2019>>
		Purpose : <<get mpr revision details based on revision id>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public MPRRevision getMPRRevisionDetails(int RevisionId)
		{
			DB.Configuration.ProxyCreationEnabled = false;

			MPRRevision mprRevisionDetails = new MPRRevision();
			mprRevisionDetails = DB.MPRRevisions.Where(li => li.RevisionId == RevisionId).FirstOrDefault<MPRRevision>();
			//mprRevisionDetails = DB.MPRRevisions.Include(x => x.MPRDetail).Include(x => x.MPRDepartment).Include(x => x.MPRProcurementSource)
			//	 .Include(x => x.MPRCustomsDuty).Include(x => x.MPRProjectDutyApplicable).Include(x => x.MPRBuyerGroup).Include(x => x.MPRItemInfoes)
			//	 .Include(x => x.MPRDocuments).Include(x => x.MPRDocumentations).Include(x => x.MPRVendorDetails).Include(x => x.MPRIncharges)
			//.Include(x => x.MPRCommunications).Include(x => x.MPR_Assignment).Where(li => li.RevisionId == RevisionId).FirstOrDefault<MPRRevision>();
			//mprRevisionDetails.MPRItemInfoes = mprRevisionDetails.MPRItemInfoes.OrderBy(li => li.Itemdetailsid).ToList();
			if (mprRevisionDetails != null)
			{
				mprRevisionDetails.MPRDetail = DB.MPRDetails.Where(li => li.RequisitionId == mprRevisionDetails.RequisitionId).FirstOrDefault<MPRDetail>();
				mprRevisionDetails.MPRDepartment = DB.MPRDepartments.Where(li => li.DepartmentId == mprRevisionDetails.DepartmentId).FirstOrDefault<MPRDepartment>();


				mprRevisionDetails.MPRProcurementSource = DB.MPRProcurementSources.Where(li => li.ProcurementSourceId == mprRevisionDetails.ProcurementSourceId).FirstOrDefault<MPRProcurementSource>();
				mprRevisionDetails.MPRCustomsDuty = DB.MPRCustomsDuties.Where(li => li.CustomsDutyId == mprRevisionDetails.CustomsDutyId).FirstOrDefault<MPRCustomsDuty>();
				mprRevisionDetails.MPRProjectDutyApplicable = DB.MPRProjectDutyApplicables.Where(li => li.ProjectDutyApplicableId == mprRevisionDetails.ProjectDutyApplicableId).FirstOrDefault<MPRProjectDutyApplicable>();


				//mprRevisionDetails.MPRScope = DB.MPRScopes.Where(li => li.ScopeId == mprRevisionDetails.ScopeId).FirstOrDefault<MPRScope>();
				mprRevisionDetails.MPRBuyerGroup = DB.MPRBuyerGroups.Where(li => li.BuyerGroupId == mprRevisionDetails.BuyerGroupId).FirstOrDefault<MPRBuyerGroup>();
				mprRevisionDetails.MPRItemInfoes = DB.MPRItemInfoes.Where(li => li.RevisionId == mprRevisionDetails.RevisionId).OrderBy(li => li.Itemdetailsid).Include(li=>li.PAItems).ToList();
				mprRevisionDetails.MPRDocuments = DB.MPRDocuments.Where(li => li.RevisionId == mprRevisionDetails.RevisionId).ToList();
				mprRevisionDetails.MPRDocumentations = DB.MPRDocumentations.Where(li => li.RevisionId == mprRevisionDetails.RevisionId).Include(li=>li.MPRDocumentationDescription).ToList();
				mprRevisionDetails.MPRVendorDetails = DB.MPRVendorDetails.Where(li => li.RevisionId == mprRevisionDetails.RevisionId).Include(li=>li.VendorMaster).ToList();
				mprRevisionDetails.MPRIncharges = DB.MPRIncharges.Where(li => li.RevisionId == mprRevisionDetails.RevisionId).ToList();
				//mprRevisionDetails.MPRCommunications = DB.MPRCommunications.Include("MPRReminderTrackings").Where(li=>li.RevisionId==mprRevisionDetails.RevisionId).ToList();
				mprRevisionDetails.MPRCommunications = DB.MPRCommunications.Where(x => x.RevisionId == mprRevisionDetails.RevisionId).Include(li=>li.Employee).Include(li=>li.MPRReminderTrackings).ToList();
				mprRevisionDetails.MPR_Assignment = DB.MPR_Assignment.Where(li => li.MprRevisionId == mprRevisionDetails.RevisionId).ToList();
			}
			//foreach (MPRItemInfo item in mprRevisionDetails.MPRItemInfoes)
			//{
			//	item.PAItems = DB.PAItems.Where(li => li.MPRItemDetailsId == item.Itemdetailsid).ToList();
			//}
			//foreach (MPRVendorDetail item in mprRevisionDetails.MPRVendorDetails)
			//{
			//	item.VendorMaster = DB.VendorMasters.Where(li => li.Vendorid == item.Vendorid).FirstOrDefault();
			//}
			//foreach (MPRDocumentation item in mprRevisionDetails.MPRDocumentations)
			//{
			//	item.MPRDocumentationDescription = DB.MPRDocumentationDescriptions.Where(li => li.DocumentationDescriptionId == item.DocumentationDescriptionId).FirstOrDefault();
			//}
			foreach (MPRCommunication item in mprRevisionDetails.MPRCommunications)
			{
				item.Employee = DB.Employees.Where(li => li.EmployeeNo == item.RemarksFrom).FirstOrDefault();
				item.MPRReminderTrackings = DB.MPRReminderTrackings.Include(x => x.Employee).Where(li => li.MPRCCId == item.MPRCCId).ToList();
			}
			return mprRevisionDetails;


		}


		/*Name of Function : <<getMPRList>>  Author :<<Prasanna>>  
		Date of Creation <<06-12-2019>>
		Purpose : <<get mpr List based on filter parameters>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public DataTable getMPRList(mprFilterParams mprfilterparams)
		{
			DataTable table = new DataTable();
			using (YSCMEntities Context = new YSCMEntities())
			{
				var query = default(string);
				//var frmDate = mprfilterparams.FromDate.ToString("yyyy-MM-dd");
				//var toDate = mprfilterparams.ToDate.ToString("yyyy-MM-dd");A
				string viewName = "left join  MPR_GetAssignEmployeList mprasgn on mprasgn.MprRevisionId = mpr.RevisionId";
				if (!string.IsNullOrEmpty(mprfilterparams.AssignEmployee))
					viewName = "inner join  MPR_GetAssignEmployee mprasgn on mprasgn.MprRevisionId = mpr.RevisionId and  mprasgn.EmployeeNo=" + mprfilterparams.AssignEmployee + "";

				if (string.IsNullOrEmpty(mprfilterparams.ItemDescription))
				{
					if (!string.IsNullOrEmpty(mprfilterparams.PONO) || !string.IsNullOrEmpty(mprfilterparams.PAID))
					{
						viewName += " inner join PAItem on PAItem.MPRItemDetailsId = mpr.Itemdetailsid ";
						query = "Select distinct RevisionId, mprasgn.EmployeeName as AssignEmployeeName,RequisitionId,ItemDescription, DocumentNo,DocumentDescription,JobCode,JobName,DepartmentName,ORgDepartmentid,IssuePurposeId,GEPSApprovalId,BuyerGroupName,PreparedBy,PreparedName,PreparedOn,CheckedBy,CheckedName,CheckedOn,CheckStatus, ApprovedBy,ApproverName,ApprovedOn,SecondApprover,SecondApproversStatus,ThirdApprover,ThirdApproverStatus,ApprovalStatus,MPRStatus,PurchaseType from MPRRevisionDetails mpr " + viewName + "  Where BoolValidRevision=1";
					}
					else
						query = "Select distinct RevisionId, mprasgn.EmployeeName as AssignEmployeeName,RequisitionId, DocumentNo,DocumentDescription,JobCode,JobName,DepartmentName,ORgDepartmentid,IssuePurposeId,GEPSApprovalId,BuyerGroupName,PreparedBy,PreparedName,PreparedOn,CheckedBy,CheckedName,CheckedOn,CheckStatus, ApprovedBy,ApproverName,ApprovedOn,SecondApprover,SecondApproversStatus,ThirdApprover,ThirdApproverStatus,ApprovalStatus,MPRStatus,PurchaseType from MPRRevisionDetails_woItems mpr " + viewName + " Where BoolValidRevision=1";
				}
				else
				{
					if (!string.IsNullOrEmpty(mprfilterparams.PONO) || !string.IsNullOrEmpty(mprfilterparams.PAID))
					{
						viewName += " inner join PAItem on PAItem.MPRItemDetailsId = mpr.Itemdetailsid ";
					}
					query = "Select distinct RevisionId,mprasgn.EmployeeName as AssignEmployeeName,RequisitionId,ItemDescription, DocumentNo,DocumentDescription,JobCode,JobName,DepartmentName,ORgDepartmentid,IssuePurposeId,GEPSApprovalId,BuyerGroupName,PreparedBy,PreparedName,PreparedOn,CheckedBy,CheckedName,CheckedOn,CheckStatus, ApprovedBy,ApproverName,ApprovedOn,SecondApprover,SecondApproversStatus,ThirdApprover,ThirdApproverStatus,ApprovalStatus,MPRStatus,PurchaseType from MPRRevisionDetails mpr " + viewName + "  Where BoolValidRevision=1";
				}
				//query = "Select * from MPRRevisionDetails Where BoolValidRevision='true' and PreparedOn <= " + mprfilterparams.ToDate.ToString() + " and PreparedOn >= " + mprfilterparams.FromDate.ToString() + "";
				if (!string.IsNullOrEmpty(mprfilterparams.ToDate))
					query += " and PreparedOn <= '" + mprfilterparams.ToDate + "'";
				if (!string.IsNullOrEmpty(mprfilterparams.FromDate))
					query += "  and PreparedOn >= '" + mprfilterparams.FromDate + "'";
				if (!string.IsNullOrEmpty(mprfilterparams.PreparedBy))
					query += " and PreparedBy = '" + mprfilterparams.PreparedBy + "'";
				if (mprfilterparams.ListType != "MPRPendingList" && !string.IsNullOrEmpty(mprfilterparams.PreparedBy))
					query += "  or RevisionId in (select RevisionId from  MPRIncharges where incharge = " + mprfilterparams.PreparedBy + ")";
					if (mprfilterparams.ListType == "MPRPendingList")
					query += " and CheckedBy ='-'";
				if (mprfilterparams.ListType == "MPRSingleVendorList")
					query += " and PurchaseTypeId =1 and  CheckStatus='Approved' and ApprovalStatus='Approved' and(SecondApprover = '" + mprfilterparams.SecOrThirdApprover + "' and SecondApproversStatus = 'Pending') or (ThirdApprover = '" + mprfilterparams.SecOrThirdApprover + "' and ThirdApproverStatus = 'Pending' and SecondApproversStatus='Approved')";
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
				if (!string.IsNullOrEmpty(mprfilterparams.ORgDepartmentid))
					query += " and ORgDepartmentid='" + mprfilterparams.ORgDepartmentid + "'";
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
				if (!string.IsNullOrEmpty(mprfilterparams.MPRStatusId))
					query += " and MPRStatusId='" + mprfilterparams.MPRStatusId + "'";
				if (!string.IsNullOrEmpty(mprfilterparams.PurchaseTypeId))
					query += " and PurchaseTypeId='" + mprfilterparams.PurchaseTypeId + "'";
				if (!string.IsNullOrEmpty(mprfilterparams.PONO))
					query += " and PONO='" + mprfilterparams.PONO + "'";
				if (!string.IsNullOrEmpty(mprfilterparams.PAID))
					query += " and PAID='" + mprfilterparams.PAID + "'";
				if (mprfilterparams.mprStatusListId != null && mprfilterparams.mprStatusListId.Count > 0)
				{
					//completed,pending
					if (mprfilterparams.mprStatusListId.Count == 2)
					{
						query += " and MPRStatusId in (1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21)";//completed and pending}

					}
					//completed
					else if (!string.IsNullOrEmpty(mprfilterparams.mprStatusListId[0]) && mprfilterparams.mprStatusListId[0] == "1")//PO Released,MPR Rejected,  MPR Closed
					{
						query += " and MPRStatusId in (12,15,19)";
					}
					//pending
					if (!string.IsNullOrEmpty(mprfilterparams.mprStatusListId[0]) && mprfilterparams.mprStatusListId[0] == "2")
					{
						query += " and MPRStatusId in (1,2,3,4,5,6,7,8,9,10,11,13,14,16,17,18,20,21)";
					}

				}
				query += " order by RevisionId desc ";
				//if (!string.IsNullOrEmpty(mprfilterparams.CheckedBy))
				//	mprRevisionDetails = DB.MPRRevisionDetails.Where(li => li.BoolValidRevision == true && (li.PreparedOn <= mprfilterparams.ToDate && li.PreparedOn >= mprfilterparams.FromDate) && (li.CheckedBy == mprfilterparams.CheckedBy) && (li.CheckStatus == mprfilterparams.Status)).OrderBy(li => li.PreparedOn).ToList();
				//else if (!string.IsNullOrEmpty(mprfilterparams.ApprovedBy))
				//	mprRevisionDetails = DB.MPRRevisionDetails.Where(li => li.BoolValidRevision == true && (li.PreparedOn <= mprfilterparams.ToDate && li.PreparedOn >= mprfilterparams.FromDate) && (li.ApprovedBy == mprfilterparams.ApprovedBy) && (li.ApprovalStatus == mprfilterparams.Status)).OrderBy(li => li.PreparedOn).ToList();
				//else
				//	mprRevisionDetails = DB.MPRRevisionDetails.Where(li => li.BoolValidRevision == true && (li.PreparedOn <= mprfilterparams.ToDate && li.PreparedOn >= mprfilterparams.FromDate)).OrderBy(li => li.PreparedOn).ToList();
				//mprRevisionDetails.ForEach(a => a.MPRDetail = DB.MPRDetails.Where(li => li.RequisitionId == a.RequisitionId).FirstOrDefault());
				var cmd = Context.Database.Connection.CreateCommand();
				cmd.CommandText = query;

				cmd.Connection.Open();
				table.Load(cmd.ExecuteReader());
				cmd.Connection.Close();
				//return Context.Database.SqlQuery<DataTable>(query);
			}
			return table;

		}

		/*Name of Function : <<getSavingsReport>>  Author :<<Prasanna>>  
		Date of Creation <<06-12-2019>>
		Purpose : <<getSavingsReport>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public DataTable getSavingsReport(mprFilterParams mprfilterparams)
		{
			DataTable table = new DataTable();
			using (YSCMEntities Context = new YSCMEntities())
			{
				var query = default(string);
				string viewName = "left join  MPR_GetAssignEmployeList mprasgn on mprasgn.MprRevisionId = mpr.RevisionId";
				if (!string.IsNullOrEmpty(mprfilterparams.AssignEmployee))
					viewName = "inner join  MPR_GetAssignEmployee mprasgn on mprasgn.MprRevisionId = mpr.RevisionId and  mprasgn.EmployeeNo=" + mprfilterparams.AssignEmployee + "";
				query = "Select mprasgn.EmployeeName as AssignEmployeeName,* from SavingsReport mpr " + viewName + "  Where BoolValidRevision=1 and PreparedOn <= '" + mprfilterparams.ToDate + "' and PreparedOn >= '" + mprfilterparams.FromDate + "'";

				if (!string.IsNullOrEmpty(mprfilterparams.DocumentNo))
					query += " and DocumentNo='" + mprfilterparams.DocumentNo + "'";
				if (!string.IsNullOrEmpty(mprfilterparams.DocumentDescription))
					query += " and DocumentDescription='" + mprfilterparams.DocumentDescription + "'";
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
				if (!string.IsNullOrEmpty(mprfilterparams.MPRStatusId))
					query += " and MPRStatusId='" + mprfilterparams.MPRStatusId + "'";
				if (!string.IsNullOrEmpty(mprfilterparams.PurchaseTypeId))
					query += " and PurchaseTypeId='" + mprfilterparams.PurchaseTypeId + "'";
				var cmd = Context.Database.Connection.CreateCommand();
				cmd.CommandText = query;

				cmd.Connection.Open();
				table.Load(cmd.ExecuteReader());
				cmd.Connection.Close();
				//return Context.Database.SqlQuery<DataTable>(query);
			}
			return table;

		}

		/*Name of Function : <<getMPRPendingListCnt>>  Author :<<Prasanna>>  
		Date of Creation <<06-04-2020>>
		Purpose : <<get MPR PendingList count>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public int getMPRPendingListCnt(string PreparedBy)
		{
			return DB.MPRRevisionDetails.Where(li => li.BoolValidRevision == true && li.PreparedBy == PreparedBy && (li.CheckedBy == "-" || li.ApprovedBy == "-")).Count();
		}

		/*Name of Function : <<getEmployeeList>>  Author :<<Prasanna>>  
		Date of Creation <<12-12-2019>>
		Purpose : <<get MPR PendingList count>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public List<Employee> getEmployeeList()
		{
			DB.Configuration.ProxyCreationEnabled = false;
			return DB.Employees.ToList();
		}

		/*Name of Function : <<getMPRRevisionList>>  Author :<<Prasanna>>  
		Date of Creation <<12-12-2019>>
		Purpose : <<get MPR Revision List>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public List<MPRRevisionDetails_woItems> getMPRRevisionList(int RequisitionId)
		{
			DB.Configuration.ProxyCreationEnabled = false;
			List<MPRRevisionDetails_woItems> mprRevisionDetails;
			mprRevisionDetails = DB.MPRRevisionDetails_woItems.Where(li => li.RequisitionId == RequisitionId).ToList();
			foreach (MPRRevisionDetails_woItems item in mprRevisionDetails)
			{
				item.MPRStatusTrackDetails = DB.MPRStatusTrackDetails.Where(li => li.RequisitionId == RequisitionId).ToList();
			}

			//mprRevisionDetails.ForEach(a => a.MPRDetail = DB.MPRDetails.Where(li => li.RequisitionId == a.RequisitionId).FirstOrDefault());
			return mprRevisionDetails;
		}

		/*Name of Function : <<statusUpdate>>  Author :<<Prasanna>>  
		Date of Creation <<26-06-2020>>
		Purpose : <<statusUpdate when Acknowledge,MPRManualStatus,Checker,approver,... status changed as approved>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public MPRRevision statusUpdate(MPRStatusUpdate mprStatus)
		{
			MPRRevision mprrevision = new MPRRevision();
			var statusId = mprStatus.StatusId;
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
						if (mprStatus.MPRAssignments.Count > 0)
						{
							statusId = mPRStatusTrackDetails.StatusId = 4;
							updateMprstatusTrack(mPRStatusTrackDetails);
							foreach (MPR_Assignment item in mprStatus.MPRAssignments)
							{
								var MPR_Assignment = Context.MPR_Assignment.Where(li => li.Employeeno == item.Employeeno && li.MprRevisionId == item.MprRevisionId).FirstOrDefault();
								if (MPR_Assignment == null)
								{
									Context.MPR_Assignment.Add(item);
									Context.SaveChanges();
								}
								else
								{
									MPR_Assignment.Employeeno = item.Employeeno;
									Context.SaveChanges();
								}
								mprrevision.StatusId = Convert.ToByte(statusId);
								Context.SaveChanges();
								this.emailTemplateDA.prepareMPRStatusEmail(mprStatus.PreparedBy, item.Employeeno, "mprAssign", mprStatus.RevisionId);
							}

						}
						if (mprStatus.BuyerGroupId != null)
						{
							mprrevision.BuyerGroupId = mprStatus.BuyerGroupId;
							Context.SaveChanges();
							string toEmailId = Context.MPRBuyerGroups.Where(li => li.BuyerGroupId == mprStatus.BuyerGroupId).FirstOrDefault().BuyerManager;
							this.emailTemplateDA.prepareMPRStatusEmail(mprStatus.PreparedBy, toEmailId, "BuyerChange", mprStatus.RevisionId);
						}
						createMPRRFQRepeatOrder(mprStatus.RevisionId);

					}
					else
					{
						if (mprStatus.typeOfuser == "MPRManualStatus")
						{
							statusId = mPRStatusTrackDetails.StatusId = mprStatus.StatusId;
							mPRStatusTrackDetails.Remarks = mprStatus.Remarks;
							updateMprstatusTrack(mPRStatusTrackDetails);
						}
						else if (mprStatus.typeOfuser == "Checker")
						{
							mprrevision.CheckStatus = mprStatus.status;
							mprrevision.CheckerRemarks = mprStatus.Remarks;
							mprrevision.CheckedOn = DateTime.Now;
							if (mprStatus.status == "Approved" || mprStatus.status == "Sent for Modification")
							{
								statusId = mPRStatusTrackDetails.StatusId = 2;
								if (mprStatus.status == "Sent for Modification")
									statusId = mPRStatusTrackDetails.StatusId = 20;//mpr send for modification
								updateMprstatusTrack(mPRStatusTrackDetails);
							}

							if (mprrevision.IssuePurposeId == 2)//update o checker
							{
								mprrevision.OCheckStatus = mprStatus.status;
								mprrevision.OCheckerRemarks = mprStatus.Remarks;
								mprrevision.OCheckedOn = DateTime.Now;
								MPRStatusTrack mPRStatusTrackDetails1 = new MPRStatusTrack();
								mPRStatusTrackDetails1.RequisitionId = mprStatus.RequisitionId;
								mPRStatusTrackDetails1.RevisionId = mprStatus.RevisionId;
								mPRStatusTrackDetails1.UpdatedBy = mprStatus.PreparedBy;
								mPRStatusTrackDetails1.UpdatedDate = DateTime.Now;
								statusId = mPRStatusTrackDetails1.StatusId = 13;//Raising PO Checked
								updateMprstatusTrack(mPRStatusTrackDetails1);
							}

						}
						else if (mprStatus.typeOfuser == "Approver")
						{
							mprrevision.ApprovalStatus = mprStatus.status;
							mprrevision.ApproverRemarks = mprStatus.Remarks;
							mprrevision.ApprovedOn = DateTime.Now;

							if (mprStatus.status == "Sent for Modification")
							{
								//mPRStatusTrackDetails.StatusId = 3;
								//if (mprStatus.status == "Sent for Modification")
								statusId = mPRStatusTrackDetails.StatusId = 20;//mpr send for modification
								updateMprstatusTrack(mPRStatusTrackDetails);
							}
							if (mprStatus.status == "Approved")
							{
								//need to check second  approver
								if (mprrevision.SecondApprover == null)
								{
									statusId = mPRStatusTrackDetails.StatusId = 3;
									updateMprstatusTrack(mPRStatusTrackDetails);
								}
							}
							if (mprrevision.IssuePurposeId == 2)//update oapprover
							{
								mprrevision.OApprovalStatus = mprStatus.status;
								mprrevision.OApproverRemarks = mprStatus.Remarks;
								mprrevision.OApprovedOn = DateTime.Now;
								MPRStatusTrack mPRStatusTrackDetails1 = new MPRStatusTrack();
								mPRStatusTrackDetails1.RequisitionId = mprStatus.RequisitionId;
								mPRStatusTrackDetails1.RevisionId = mprStatus.RevisionId;
								mPRStatusTrackDetails1.UpdatedBy = mprStatus.PreparedBy;
								mPRStatusTrackDetails1.UpdatedDate = DateTime.Now;
								statusId = mPRStatusTrackDetails1.StatusId = 14;//Raising PO Approved
								updateMprstatusTrack(mPRStatusTrackDetails1);
							}
						}
						else if (mprStatus.typeOfuser == "SecondApprover")
						{
							mprrevision.SecondApproversStatus = mprStatus.status;
							mprrevision.SecondApproverRemarks = mprStatus.Remarks;
							mprrevision.SecondApprovedOn = DateTime.Now;
							//need to check third  approver
							if (mprrevision.ThirdApprover == null)
							{
								statusId = mPRStatusTrackDetails.StatusId = 3;
								updateMprstatusTrack(mPRStatusTrackDetails);
							}
							mprrevision.OSecondApproversStatus = mprStatus.status;
							mprrevision.OSecondApproverRemarks = mprStatus.Remarks;
							mprrevision.OSecondApprovedOn = DateTime.Now;

						}
						else if (mprStatus.typeOfuser == "ThirdApprover")
						{
							mprrevision.ThirdApproverStatus = mprStatus.status;
							mprrevision.ThirdApproverRemarks = mprStatus.Remarks;
							mprrevision.ThirdApproverStatusChangedOn = DateTime.Now;
							statusId = mPRStatusTrackDetails.StatusId = 3;
							updateMprstatusTrack(mPRStatusTrackDetails);

							mprrevision.OThirdApproverStatus = mprStatus.status;
							mprrevision.OThirdApproverRemarks = mprStatus.Remarks;
							mprrevision.OThirdApproverStatusChangedOn = DateTime.Now;
						}
						else if (mprStatus.typeOfuser == "OChecker")
						{
							mprrevision.OCheckStatus = mprStatus.status;
							mprrevision.OCheckerRemarks = mprStatus.Remarks;
							mprrevision.OCheckedOn = DateTime.Now;
							if (mprStatus.status == "Approved")
							{
								statusId = mPRStatusTrackDetails.StatusId = 13;
								updateMprstatusTrack(mPRStatusTrackDetails);
							}
						}
						else if (mprStatus.typeOfuser == "OApprover")
						{
							mprrevision.OApprovalStatus = mprStatus.status;
							mprrevision.OApproverRemarks = mprStatus.Remarks;
							mprrevision.OApprovedOn = DateTime.Now;
							if (mprStatus.status == "Approved")
							{
								statusId = mPRStatusTrackDetails.StatusId = 14;
								updateMprstatusTrack(mPRStatusTrackDetails);
							}
						}
						else if (mprStatus.typeOfuser == "OSecondApprover")
						{
							mprrevision.OSecondApproversStatus = mprStatus.status;
							mprrevision.OSecondApproverRemarks = mprStatus.Remarks;
							mprrevision.OSecondApprovedOn = DateTime.Now;
						}
						else if (mprStatus.typeOfuser == "OThirdApprover")
						{
							mprrevision.OThirdApproverStatus = mprStatus.status;
							mprrevision.OThirdApproverRemarks = mprStatus.Remarks;
							mprrevision.OThirdApproverStatusChangedOn = DateTime.Now;
						}
						mprrevision.StatusId = Convert.ToByte(statusId);
						Context.SaveChanges();
					}
					Context.SaveChanges();

					this.emailTemplateDA.prepareMPREmailTemplate(mprStatus.typeOfuser, mprStatus.RevisionId, "", "", "");
					if (mprStatus.typeOfuser == "Checker" || mprStatus.typeOfuser == "Approver" || mprStatus.typeOfuser == "SecondApprover" || mprStatus.typeOfuser == "ThirdApprover")
					{

					}
					else
					{
						//send mail to requestor for manual status,acknoweldge,buyer group change
						this.emailTemplateDA.mailtoRequestor(mprrevision.RevisionId, mprrevision.PreparedBy);
					}
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
						log.ErrorMessage("MPRController", "statusUpdate", ve.ErrorMessage);
						Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
							ve.PropertyName, ve.ErrorMessage);
					}
				}
			}
			return this.getMPRRevisionDetails(mprStatus.RevisionId);
		}

		/*Name of Function : <<createMPRRFQRepeatOrder>>  Author :<<Prasanna>>  
		Date of Creation <<30-03-2020>>
		Purpose : <<create mprrfq for repeat order>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public void createMPRRFQRepeatOrder(int revisionid)
		{

			using (YSCMEntities Context = new YSCMEntities())
			{
				List<MPRItemInfo> list = Context.MPRItemInfoes.Where(li => li.RevisionId == revisionid).ToList();

				foreach (MPRItemInfo item in list)
				{
					List<MPRRfqItem> mprItems = Context.MPRRfqItems.Where(li => li.MPRItemDetailsid == item.RepeatOrderRefId).ToList();
					foreach (MPRRfqItem mprrfqItem in mprItems)
					{
						if (mprrfqItem != null)
						{
							MPRRfqItem mprItem = new MPRRfqItem();
							mprItem.MPRRevisionId = revisionid;
							mprItem.MPRItemDetailsid = item.Itemdetailsid;
							mprItem.RfqItemsid = mprrfqItem.RfqItemsid;
							Context.MPRRfqItems.Add(mprItem);
							Context.SaveChanges();
							MPRRfqItemInfo mprItemInfo = Context.MPRRfqItemInfos.Where(li => li.MPRRFQitemId == mprrfqItem.MPRRFQitemId).FirstOrDefault();
							if (mprItemInfo != null)
							{
								mprItemInfo = new MPRRfqItemInfo();
								mprItemInfo.MPRRFQitemId = mprItem.MPRRFQitemId;
								mprItemInfo.rfqsplititemid = mprItemInfo.rfqsplititemid;
								Context.MPRRfqItemInfos.Add(mprItemInfo);
								Context.SaveChanges();

							}
						}

					}


				}
			}
		}

		/*Name of Function : <<getStatusList>>  Author :<<Prasanna>>  
		Date of Creation <<30-03-2020>>
		Purpose : <<get mpr status list>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public List<SCMStatu> getStatusList()
		{
			using (YSCMEntities Context = new YSCMEntities())
			{
				return Context.SCMStatus.Where(li => li.deleteFlag == false).ToList();
			}
		}

		/*Name of Function : <<getAccessList>>  Author :<<Prasanna>>  
		Date of Creation <<30-03-2020>>
		Purpose : <<get  Access list of userpermissions>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public List<UserPermission> getAccessList(int RoleId)
		{
			using (YSCMEntities context = new YSCMEntities())
			{
				return context.UserPermissions.Where(li => li.RoleId == RoleId).ToList();
			}
		}

		/*Name of Function : <<updateMPRVendor>>  Author :<<Prasanna>>  
		Date of Creation <<30-03-2020>>
		Purpose : <<update mpr vendor details>>
		Review Date :<<>>   Reviewed By :<<>>*/
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

		/*Name of Function : <<updateMprstatusTrack>>  Author :<<Prasanna>>  
		Date of Creation <<30-03-2020>>
		Purpose : <<update mpr status track details>>
		Review Date :<<>>   Reviewed By :<<>>*/
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

		/*Name of Function : <<deleteMPR>>  Author :<<Prasanna>>  
		Date of Creation <<30-03-2020>>
		Purpose : <<delete mpr revision by setting delete flag as true>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public bool deleteMPR(DeleteMpr deleteMprInfo)
		{

			using (YSCMEntities Context = new YSCMEntities())
			{
				MPRRevision mPRRevision = Context.MPRRevisions.Where(li => li.RevisionId == deleteMprInfo.RevisionId).FirstOrDefault();
				mPRRevision.DeleteFlag = true;
				mPRRevision.DeletedRemarks = deleteMprInfo.DeletedRemarks;
				mPRRevision.DeletedBy = deleteMprInfo.Deletedby;
				mPRRevision.DeletedOn = DateTime.Now;
				Context.SaveChanges();
			}
			return true;
		}

		/*Name of Function : <<GeneratePassword>>  Author :<<Prasanna>>  
		Date of Creation <<30-03-2020>>
		Purpose : <<Genereate password for vendor user>>
		Review Date :<<>>   Reviewed By :<<>>*/
		public static string GeneratePassword()
		{
			bool includeLowercase = true;
			bool includeUppercase = true;
			bool includeNumeric = true;
			bool includeSpecial = true;
			bool includeSpaces = false;
			int lengthOfPassword = 8;
			const int MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS = 2;
			const string LOWERCASE_CHARACTERS = "abcdefghijklmnopqrstuvwxyz";
			const string UPPERCASE_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			const string NUMERIC_CHARACTERS = "0123456789";
			const string SPECIAL_CHARACTERS = @"!#$%*@\";
			const string SPACE_CHARACTER = " ";
			const int PASSWORD_LENGTH_MIN = 8;
			const int PASSWORD_LENGTH_MAX = 128;

			if (lengthOfPassword < PASSWORD_LENGTH_MIN || lengthOfPassword > PASSWORD_LENGTH_MAX)
			{
				return "Password length must be between 8 and 128.";
			}

			string characterSet = "";

			if (includeLowercase)
			{
				characterSet += LOWERCASE_CHARACTERS;
			}

			if (includeUppercase)
			{
				characterSet += UPPERCASE_CHARACTERS;
			}

			if (includeNumeric)
			{
				characterSet += NUMERIC_CHARACTERS;
			}

			if (includeSpecial)
			{
				characterSet += SPECIAL_CHARACTERS;
			}

			if (includeSpaces)
			{
				characterSet += SPACE_CHARACTER;
			}

			char[] password = new char[lengthOfPassword];
			int characterSetLength = characterSet.Length;

			System.Random random = new System.Random();
			for (int characterPosition = 0; characterPosition < lengthOfPassword; characterPosition++)
			{
				password[characterPosition] = characterSet[random.Next(characterSetLength - 1)];

				bool moreThanTwoIdenticalInARow =
					characterPosition > MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS
					&& password[characterPosition] == password[characterPosition - 1]
					&& password[characterPosition - 1] == password[characterPosition - 2];

				if (moreThanTwoIdenticalInARow)
				{
					characterPosition--;
				}
			}

			return string.Join(null, password);
		}



	}
}
