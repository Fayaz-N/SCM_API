using SCMModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;


namespace DALayer.MPR
{
	public class MPRDA : IMPRDA
	{
		YSCMEntities DB = new YSCMEntities();

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
					int requestionId = 0;
					//mpr.RevisionId = 0;
					//mpr.RequisitionId = 224;
					if (mpr.RevisionId == 0 && mpr.RequisitionId != 0 && !string.IsNullOrEmpty(mpr.MPRDetail.DocumentNo))
					{
						MPRRevision mprLastRecord = DB.MPRRevisions.OrderByDescending(p => p.RevisionId).Where(li => li.RequisitionId == mpr.RequisitionId).FirstOrDefault<MPRRevision>();
						mprLastRecord.BoolValidRevision = false;
						DB.SaveChanges();
						mprRevisionDetails = mpr;
						mprRevisionDetails.RevisionNo = Convert.ToByte(mprLastRecord.RevisionNo + 1);
						mprRevisionDetails.BoolValidRevision = true;
						mprRevisionDetails.MPRDetail = null;
						mprRevisionDetails.ApprovalStatus = mprRevisionDetails.CheckStatus=mprRevisionDetails.SecondApproversStatus=mprRevisionDetails.ThirdApproverStatus = "Pending";
						DB.MPRRevisions.Add(mprRevisionDetails);
						DB.SaveChanges();
						requestionId = mprDetails.RequisitionId;
					}
					mprRevisionDetails = DB.MPRRevisions.Where(li => li.RevisionId == mpr.RevisionId && li.RequisitionId == mpr.RequisitionId).FirstOrDefault<MPRRevision>();
					if (mprRevisionDetails == null)
					{
						mpr.MPRDetail.DocumentNo = "MPR/" + DateTime.Now.ToString("MMyy") + "/" + DateTime.Now.ToString("hmmss");
						mpr.MPRDetail.SubmittedBy = "User";
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
									item.ItemDescription = mPRItemInfo.ItemDescription;
									item.Quantity = mPRItemInfo.Quantity;
									item.UnitId = mPRItemInfo.UnitId;
									item.SOLineItemNo = mPRItemInfo.SOLineItemNo;
									item.ReferenceDocNo = mPRItemInfo.ReferenceDocNo;
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
								item.UploadedBy = "User";
								item.UplaodedDate = DateTime.Now;
								item.Path = "";
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
							foreach (MPRVendorDetail item in mpr.MPRVendorDetails)
							{
								item.RevisionId = mprRevisionDetails.RevisionId;
								//item.RevisionId = Convert.ToInt32(mprRevisionDetails.MPRItemInfoes.FirstOrDefault().RevisionId);

								item.UpdatedBy = "User";
								item.UpdatedDate = DateTime.Now;
								item.RemoveFlag = false;
								if (item.VendorDetailsId == 0)
									mprRevisionDetails.MPRVendorDetails.Add(item);
								else
								{
									MPRVendorDetail MPRVendordetails = DB.MPRVendorDetails.Where(li => li.RevisionId == mprRevisionDetails.RevisionId && li.VendorDetailsId == item.VendorDetailsId).FirstOrDefault();

									MPRVendordetails.Vendorid = item.Vendorid;
									DB.SaveChanges();
								}

							}
							DB.SaveChanges();
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
								item.UpdatedBy = "User";
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
			return mprRevisionDetails;


		}

		public List<MPRRevision> getMPRList(mprFilterParams mprfilterparams)
		{
			List<MPRRevision> mprRevisionDetails;
			using (var db = new YSCMEntities()) //ok
			{
				//YSCMEntities DB = new YSCMEntities();

				DB.Configuration.ProxyCreationEnabled = false;
				if (!string.IsNullOrEmpty(mprfilterparams.CheckedBy))
					mprRevisionDetails = DB.MPRRevisions.Where(li => li.BoolValidRevision == true && (li.PreparedOn <= mprfilterparams.ToDate && li.PreparedOn >= mprfilterparams.FromDate) && (li.CheckedBy == mprfilterparams.CheckedBy) && (li.CheckStatus == mprfilterparams.Status)).Include(x => x.MPRDetail).ToList();
				else if (!string.IsNullOrEmpty(mprfilterparams.ApprovedBy))
					mprRevisionDetails = DB.MPRRevisions.Where(li => li.BoolValidRevision == true && (li.PreparedOn <= mprfilterparams.ToDate && li.PreparedOn >= mprfilterparams.FromDate) && (li.ApprovedBy == mprfilterparams.ApprovedBy) && (li.ApprovalStatus == mprfilterparams.Status)).Include(x => x.MPRDetail).ToList();
				else
					mprRevisionDetails = DB.MPRRevisions.Where(li => li.BoolValidRevision == true && (li.PreparedOn <= mprfilterparams.ToDate && li.PreparedOn >= mprfilterparams.FromDate)).Include(x => x.MPRDetail).ToList();
				//mprRevisionDetails.ForEach(a => a.MPRDetail = DB.MPRDetails.Where(li => li.RequisitionId == a.RequisitionId).FirstOrDefault());

			}
			return mprRevisionDetails;
		}

		public List<Employee> getEmployeeList()
		{
			DB.Configuration.ProxyCreationEnabled = false;
			return DB.Employees.ToList();
		}

		public List<MPRRevision> getMPRRevisionList(int RequisitionId)
		{
			DB.Configuration.ProxyCreationEnabled = false;
			List<MPRRevision> mprRevisionDetails;
			mprRevisionDetails = DB.MPRRevisions.Where(li => li.RequisitionId == RequisitionId).ToList();
			//mprRevisionDetails.ForEach(a => a.MPRDetail = DB.MPRDetails.Where(li => li.RequisitionId == a.RequisitionId).FirstOrDefault());
			return mprRevisionDetails;
		}

		public MPRRevision statusUpdate(MPRStatusUpdate mprStatus)
		{
			MPRRevision mprrevision = new MPRRevision();
			try
			{
				using (YSCMEntities Context = new YSCMEntities())
				{
					 mprrevision = Context.MPRRevisions.Find(mprStatus.RevisionId);
					if (mprStatus.typeOfuser == "Checker")
					{
						mprrevision.CheckStatus = mprStatus.status;
						mprrevision.CheckerRemarks = mprStatus.Remarks;
						mprrevision.CheckedOn = DateTime.Now;
					}
					else if (mprStatus.typeOfuser == "Approver")
					{
						mprrevision.ApprovalStatus = mprStatus.status;
						mprrevision.ApproverRemarks = mprStatus.Remarks;
						mprrevision.ApprovedOn = DateTime.Now;
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
			return DB.SCMStatus.ToList();
		}

	}
}
