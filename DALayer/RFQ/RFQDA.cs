using DALayer.Emails;
using DALayer.MPR;
using SCMModels;
using SCMModels.MPRMasterModels;
using SCMModels.RemoteModel;
using SCMModels.RFQModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALayer.RFQ
{
    public class RFQDA : IRFQDA
    {
        private IMPRDA MPRDA = default(IMPRDA);
        private IEmailTemplateDA emailTemplateDA = default(IEmailTemplateDA);
        public RFQDA(IMPRDA MPRDA, IEmailTemplateDA EmailTemplateDA)
        {
            this.MPRDA = MPRDA;
            this.emailTemplateDA = EmailTemplateDA;
        }


        VSCMEntities vscm = new VSCMEntities();
        YSCMEntities obj = new YSCMEntities();

        public List<RFQQuoteView> getRFQItems(int RevisionId)
        {
            using (YSCMEntities Context = new YSCMEntities())
            {
                string query = "select  *  from RFQQuoteView where MPRRevisionId=" + RevisionId + " ORDER BY  RFQQuoteView.ItemId, RFQQuoteView.UnitPrice";
                return Context.Database.SqlQuery<RFQQuoteView>(query).ToList();
                //return Context.RFQQuoteViews.Where(li => li.MPRRevisionId == RevisionId).ToList();
            }
        }
        public bool updateVendorQuotes(List<RFQQuoteView> RFQQuoteViewList, List<YILTermsandCondition> termsList)
        {
            List<RFQTermsModel> rfqList = new List<RFQTermsModel>();
            foreach (var data in termsList)
            {
                if (data.DefaultSelect == true)
                {
                    RFQTermsModel rfqterm = new RFQTermsModel();
                    rfqterm.termsid = data.TermId;
                    rfqterm.TermGroup = obj.YILTermsGroups.Where(li => li.TermGroupId == data.TermGroupId).FirstOrDefault<YILTermsGroup>().TermGroup;
                    rfqterm.Terms = data.Terms;
                    rfqterm.CreatedBy = data.CreatedBy;
                    rfqterm.CreatedDate = DateTime.Now;
                    rfqList.Add(rfqterm);
                }

            }
            List<RFQQuoteView> mainList = RFQQuoteViewList.GroupBy(p => p.VendorId)
                           .Select(grp => grp.First())
                           .ToList();
            foreach (RFQQuoteView item in mainList)
            {

                RfqRevisionModel rfqModel = new RfqRevisionModel();
                rfqModel.rfqmaster = new RFQMasterModel();
                rfqModel.rfqmaster.MPRRevisionId = Convert.ToInt32(item.MPRRevisionId);
                rfqModel.rfqmaster.VendorId = item.VendorId;
                rfqModel.rfqmaster.CreatedBy = item.CreatedBy;
                rfqModel.rfqmaster.Created = DateTime.Now;
                rfqModel.CreatedBy = Convert.ToInt32(item.CreatedBy);
                rfqModel.CreatedDate = DateTime.Now;
                rfqModel.RfqValidDate = Convert.ToDateTime(item.RFQValidDate);
                rfqModel.PackingForwading = item.PackingForwarding;
                rfqModel.ExciseDuty = item.ExciseDuty;
                rfqModel.salesTax = item.SalesTax;
                rfqModel.freight = item.Freight;
                rfqModel.Insurance = item.Insurance;
                rfqModel.CustomsDuty = item.CustomsDuty;
                rfqModel.ShipmentModeId = item.ShipmentModeid;
                rfqModel.PaymentTermDays = item.PaymentTermDays;
                rfqModel.PaymentTermRemarks = item.PaymentTermRemarks;
                rfqModel.BankGuarantee = item.BankGuarantee;
                rfqModel.DeliveryMinWeeks = item.DeliveryMinWeeks;
                rfqModel.DeliveryMaxWeeks = item.DeliveryMaxWeeks;

                var itemList = RFQQuoteViewList.Where(li => li.VendorId == item.VendorId).ToList();
                foreach (RFQQuoteView sitem in itemList)
                {
                    RfqItemModel rfqitem = new RfqItemModel();
                    rfqitem.MRPItemsDetailsID = Convert.ToInt32(sitem.MPRItemDetailsid);
                    rfqitem.ItemId = sitem.ItemId;
                    rfqitem.ItemName = sitem.ItemName;
                    rfqitem.ItemDescription = sitem.ItemDescription;
                    rfqitem.QuotationQty = Convert.ToDouble(sitem.QuotationQty);
                    rfqModel.rfqitem.Add(rfqitem);
                }
                rfqModel.RFQTerms = rfqList;
                CreateRfQ(rfqModel);
                MPRStatusTrack mPRStatusTrackDetails = new MPRStatusTrack();
                mPRStatusTrackDetails.RequisitionId = obj.MPRRevisions.Where(li => li.RevisionId == item.MPRRevisionId).FirstOrDefault().RequisitionId;
                mPRStatusTrackDetails.RevisionId = Convert.ToInt32(item.MPRRevisionId);
                mPRStatusTrackDetails.StatusId = 7;
                mPRStatusTrackDetails.UpdatedBy = item.CreatedBy;
                mPRStatusTrackDetails.UpdatedDate = DateTime.Now;
                this.MPRDA.updateMprstatusTrack(mPRStatusTrackDetails);
                this.emailTemplateDA.prepareRFQGeneratedEmail(rfqModel.rfqmaster.CreatedBy, item.VendorId);
            }
            return true;

        }

        public DataSet getRFQCompareItems(int RevisionId)
        {
            DataSet dataset = new DataSet();
            DataTable compareDet = new DataTable();
            compareDet.TableName = "CompareTable";
            DataTable rfqterms = new DataTable();
            rfqterms.TableName = "RfqtermsTable";

            using (YSCMEntities Context = new YSCMEntities())
            {

                //string query = "select mprdet.DocumentNo,mprdet.DocumentDescription,mprdet.IssuePurposeId,mprdet.DepartmentName,mprdet.ProjectManagerName,mprdet.JobCode,mprdet.JobName,mprdet.GEPSApprovalId,mprdet.SaleOrderNo,mprdet.ClientName,mprdet.PlantLocation,mprdet.BuyerGroupName, * from RFQQuoteView inner join MPRRevisionDetails mprdet on mprdet.RevisionId = RFQQuoteView.MPRRevisionId where (Status not like '%Approved%' or Status is null) and MPRRevisionId=" + RevisionId + "";
                string query = "select mprdet.DocumentNo,mprdet.DocumentDescription,mprdet.IssuePurposeId,mprdet.DepartmentName,mprdet.ProjectManagerName,mprdet.JobCode,mprdet.JobName,mprdet.GEPSApprovalId,mprdet.SaleOrderNo,mprdet.ClientName,mprdet.PlantLocation,mprdet.BuyerGroupName, * from RFQCompareView inner join MPRRevisionDetails mprdet on mprdet.RevisionId = RFQCompareView.MPRRevisionId where  MPRRevisionId=" + RevisionId + "";
                var cmd = Context.Database.Connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Connection.Open();
                compareDet.Load(cmd.ExecuteReader());

                string rfqQuery = "select  rt.RFQrevisionId,  yt.Terms,rt.VendorResponse,rt.Remarks from RFQTerms rt  inner join YILTermsandConditions yt on yt.TermId= rt.termsid where RFQrevisionId in( select rfqRevisionId from RFQQuoteView where MPRRevisionId=" + RevisionId + ") order by rt.RFQrevisionId";
                var cmd1 = Context.Database.Connection.CreateCommand();
                cmd1.CommandText = rfqQuery;
                rfqterms.Load(cmd1.ExecuteReader());

                dataset.Tables.Add(compareDet);
                dataset.Tables.Add(rfqterms);
                cmd.Connection.Close();
            }
            return dataset;
        }
        public DataTable gerRFQVendorQuoteDetails(int rfqRevisionId)
        {
            DataTable table = new DataTable();

            using (YSCMEntities Context = new YSCMEntities())
            {

                string query = "select mprdet.DocumentNo,mprdet.DocumentDescription,mprdet.IssuePurposeId,mprdet.DepartmentName,mprdet.ProjectManagerName,mprdet.JobCode,mprdet.JobName,mprdet.GEPSApprovalId,mprdet.SaleOrderNo,mprdet.ClientName,mprdet.PlantLocation,mprdet.BuyerGroupName, * from RFQQuoteView inner join MPRRevisionDetails mprdet on mprdet.RevisionId = RFQQuoteView.MPRRevisionId where (Status not like '%Approved%' or Status is null) and rfqRevisionId=" + rfqRevisionId + "";
                var cmd = Context.Database.Connection.CreateCommand();
                cmd.CommandText = query;

                cmd.Connection.Open();
                table.Load(cmd.ExecuteReader());
                cmd.Connection.Close();
            }
            return table;
        }
        public bool rfqStatusUpdate(List<RFQQuoteView> vendorList)
        {
            using (YSCMEntities Context = new YSCMEntities())
            {
                foreach (var item in vendorList)
                {
                    RFQItemsInfo_N rfqItem = Context.RFQItemsInfo_N.Where(li => li.RFQSplitItemId == item.RFQSplitItemId).FirstOrDefault<RFQItemsInfo_N>();
                    rfqItem.Status = "Approved";
                    rfqItem.StatusUpdatedBy = item.CreatedBy;
                    rfqItem.StatusUpdateddate = DateTime.Now;
                    Context.SaveChanges();

                }
            }
            return true;
        }
        public async Task<statuscheckmodel> CreateRfQ(RfqRevisionModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                //server data
                var rfqremote = new RemoteRFQMaster();
                if (model != null)
                {

                    //vscm.Database.Connection.Open();
                    if (model.rfqmaster.RfqMasterId == 0)
                    {
                        Int64 sequenceNo = Convert.ToInt64(vscm.RemoteRFQMasters.Max(li => li.RFQUniqueNo));
                        if (sequenceNo == null || sequenceNo == 0)
                            sequenceNo = 1;
                        else
                        {
                            sequenceNo = sequenceNo + 1;
                        }
                        var value = obj.SP_sequenceNumber(sequenceNo).FirstOrDefault();
                        rfqremote.RFQNo = "RFQ/" + DateTime.Now.ToString("MMyy") + "/" + value;
                        rfqremote.RFQUniqueNo = Convert.ToInt32(sequenceNo);
                        rfqremote.MPRRevisionId = model.rfqmaster.MPRRevisionId;
                        //rfqremote.RFQUniqueNo = model.rfqmaster.RfqUniqueNo;
                        rfqremote.CreatedBy = model.rfqmaster.CreatedBy;
                        rfqremote.CreatedDate = model.rfqmaster.Created;
                        rfqremote.VendorId = model.rfqmaster.VendorId;
                        vscm.RemoteRFQMasters.Add(rfqremote);
                        vscm.SaveChanges();
                    }
                    else
                    {
                        rfqremote.RFQUniqueNo = model.rfqmaster.RfqUniqueNo;
                        //rfqdomain.RfqMasterId = model.RfqMasterId;
                        rfqremote.MPRRevisionId = model.rfqmaster.MPRRevisionId;
                        rfqremote.VendorId = model.rfqmaster.VendorId;
                        rfqremote.CreatedBy = model.rfqmaster.CreatedBy;
                        rfqremote.CreatedDate = model.rfqmaster.Created;
                        try
                        {
                            vscm.RemoteRFQMasters.Add(rfqremote);
                            vscm.SaveChanges();
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }

                    int id = rfqremote.RfqMasterId;

                    RemoteRFQRevisions_N revision = new RemoteRFQRevisions_N();
                    if (model.RfqRevisionId == 0)
                    {
                        revision.rfqMasterId = id;
                        revision.RevisionNo = model.RfqRevisionNo;
                        revision.ActiveRevision = true;
                        revision.CreatedBy = model.CreatedBy;
                        revision.CreatedDate = model.CreatedDate;
                        revision.RFQValidDate = model.RfqValidDate;
                        revision.SalesTax = model.salesTax;
                        revision.PaymentTermRemarks = model.PaymentTermRemarks;
                        revision.PackingForwarding = model.PackingForwading;
                        revision.PaymentTermDays = model.PaymentTermDays;
                        revision.Insurance = model.Insurance;
                        revision.ExciseDuty = model.ExciseDuty;
                        revision.ShipmentModeid = model.ShipmentModeId;
                        revision.BankGuarantee = model.BankGuarantee;
                        revision.DeliveryMaxWeeks = model.DeliveryMaxWeeks;
                        revision.DeliveryMinWeeks = model.DeliveryMinWeeks;
                        //revision.RemoteRFQStatus.Select(x => new Remotedata.RFQStatu()
                        //{
                        //    StatusId = Convert.ToInt32(Enum.GetName(typeof(RFQStatusType), RFQStatusType.requested))
                        //});

                        try
                        {
                            vscm.RemoteRFQRevisions_N.Add(revision);
                            vscm.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                    else
                    {
                        revision.RevisionNo = model.RfqRevisionNo;
                        revision.CreatedBy = model.CreatedBy;
                        revision.CreatedDate = model.CreatedDate;
                        revision.RFQValidDate = model.RfqValidDate;
                        revision.SalesTax = model.salesTax;
                        revision.PaymentTermRemarks = model.PaymentTermRemarks;
                        revision.PackingForwarding = model.PackingForwading;
                        revision.PaymentTermDays = model.PaymentTermDays;
                        revision.Insurance = model.Insurance;
                        revision.ExciseDuty = model.ExciseDuty;
                        revision.ShipmentModeid = model.ShipmentModeId;
                        revision.BankGuarantee = model.BankGuarantee;
                        revision.DeliveryMaxWeeks = model.DeliveryMaxWeeks;
                        revision.DeliveryMinWeeks = model.DeliveryMinWeeks;

                        try
                        {
                            vscm.RemoteRFQRevisions_N.Add(revision);
                            vscm.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }


                    int revisionid = revision.rfqRevisionId;

                    foreach (var data in model.rfqitem)
                    {

                        var rfitems = new RemoteRFQItems_N()
                        {
                            RFQRevisionId = revisionid,
                            ItemId = data.ItemId,
                            MPRItemDetailsid = data.MRPItemsDetailsID,
                            QuotationQty = data.QuotationQty,
                            VendorModelNo = data.VendorModelNo,
                            HSNCode = data.HSNCode,
                            RequestRemarks = data.RequsetRemarks,
                            ItemName = data.ItemName,
                            ItemDescription = data.ItemDescription,

                        };
                        vscm.RemoteRFQItems_N.Add(rfitems);
                        vscm.SaveChanges();
                        List<MPRDocument> mprdocumnts = obj.MPRDocuments.Where(li => li.ItemDetailsId == data.MRPItemsDetailsID).ToList();

                        foreach (var item in mprdocumnts)
                        {
                            RemoteRFQDocument rfqDoc = new RemoteRFQDocument();
                            rfqDoc.rfqRevisionId = revisionid;
                            rfqDoc.rfqItemsid = rfitems.RFQItemsId;
                            rfqDoc.DocumentName = item.DocumentName;
                            rfqDoc.DocumentType = item.DocumentTypeid;
                            rfqDoc.Path = item.Path;
                            rfqDoc.UploadedBy = Convert.ToString(revision.CreatedBy);
                            rfqDoc.uploadedDate = DateTime.Now;
                            try
                            {
                                vscm.RemoteRFQDocuments.Add(rfqDoc);
                                vscm.SaveChanges();
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }
                    }
                }

                int masterid = rfqremote.RfqMasterId;
                string rfeNo = rfqremote.RFQNo;
                int remoterevisionid = (from x in vscm.RemoteRFQRevisions_N orderby x.rfqRevisionId descending select x.rfqRevisionId).First();
                int Ritemid = (from x in vscm.RemoteRFQItems_N orderby x.RFQItemsId descending select x.RFQItemsId).First();
                //vscm.Database.Connection.Close();
                if (model != null)
                {
                    //obj.Database.Connection.Open();
                    var rfqlocal = new RFQMaster();
                    if (model.rfqmaster.RfqMasterId == 0)
                    {
                        Int64 sequenceNo = Convert.ToInt64(obj.RFQMasters.Max(li => li.RFQUniqueNo));
                        if (sequenceNo == null || sequenceNo == 0)
                            sequenceNo = 1;
                        else
                        {
                            sequenceNo = sequenceNo + 1;
                        }
                        var value = obj.SP_sequenceNumber(sequenceNo).FirstOrDefault();
                        rfqlocal.RFQNo = rfeNo;
                        rfqlocal.RFQUniqueNo = Convert.ToInt32(sequenceNo);
                        rfqlocal.MPRRevisionId = model.rfqmaster.MPRRevisionId;
                        rfqlocal.RfqMasterId = masterid;
                        // rfqlocal.RFQUniqueNo = model.rfqmaster.RfqUniqueNo;
                        rfqlocal.CreatedBy = model.rfqmaster.CreatedBy;
                        rfqlocal.CreatedDate = model.rfqmaster.Created;
                        rfqlocal.VendorId = model.rfqmaster.VendorId;
                        try
                        {
                            obj.RFQMasters.Add(rfqlocal);
                            obj.SaveChanges();
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                    else
                    {
                        rfqlocal.RFQUniqueNo = model.rfqmaster.RfqUniqueNo;
                        rfqlocal.MPRRevisionId = model.rfqmaster.MPRRevisionId;
                        //rfqdomain.RfqMasterId = model.RfqMasterId;
                        rfqlocal.VendorId = model.rfqmaster.VendorId;
                        rfqlocal.CreatedBy = model.rfqmaster.CreatedBy;
                        rfqlocal.CreatedDate = model.rfqmaster.Created;
                        try
                        {
                            obj.RFQMasters.Add(rfqlocal);
                            obj.SaveChanges();
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }

                    RFQRevisions_N revision = new RFQRevisions_N();
                    if (model.RfqRevisionId == 0)
                    {
                        revision.rfqRevisionId = remoterevisionid;
                        revision.ActiveRevision = true;
                        revision.rfqMasterId = masterid;
                        revision.RevisionNo = model.RfqRevisionNo;
                        revision.CreatedBy = model.CreatedBy;
                        revision.CreatedDate = model.CreatedDate;
                        revision.RFQValidDate = model.RfqValidDate;
                        revision.SalesTax = model.salesTax;
                        revision.PaymentTermRemarks = model.PaymentTermRemarks;
                        revision.PackingForwarding = model.PackingForwading;
                        revision.PaymentTermDays = model.PaymentTermDays;
                        revision.Insurance = model.Insurance;
                        revision.ExciseDuty = model.ExciseDuty;
                        revision.ShipmentModeid = model.ShipmentModeId;
                        revision.BankGuarantee = model.BankGuarantee;
                        revision.DeliveryMaxWeeks = model.DeliveryMaxWeeks;
                        revision.DeliveryMinWeeks = model.DeliveryMinWeeks;
                        revision.DeleteFlag = false;
                        //revision.RFQStatus.Select(x => new RFQStatu()
                        //{
                        //    StatusId = Convert.ToInt32(Enum.GetName(typeof(RFQStatusType), RFQStatusType.requested))
                        //});
                        try
                        {
                            obj.RFQRevisions_N.Add(revision);
                            obj.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }

                    }
                    else
                    {
                        revision.RevisionNo = model.RfqRevisionNo;
                        revision.CreatedBy = model.CreatedBy;
                        revision.CreatedDate = model.CreatedDate;
                        revision.RFQValidDate = model.RfqValidDate;
                        revision.SalesTax = model.salesTax;
                        revision.PaymentTermRemarks = model.PaymentTermRemarks;
                        revision.PackingForwarding = model.PackingForwading;
                        revision.PaymentTermDays = model.PaymentTermDays;
                        revision.Insurance = model.Insurance;
                        revision.ExciseDuty = model.ExciseDuty;
                        revision.ShipmentModeid = model.ShipmentModeId;
                        revision.BankGuarantee = model.BankGuarantee;
                        revision.DeliveryMaxWeeks = model.DeliveryMaxWeeks;
                        revision.DeliveryMinWeeks = model.DeliveryMinWeeks;

                        try
                        {
                            obj.RFQRevisions_N.Add(revision);
                            obj.SaveChanges();
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }


                    int revisionid = revision.rfqRevisionId;
                    var rfqitems = vscm.RemoteRFQItems_N.Where(li => li.RFQRevisionId == remoterevisionid).ToList();
                    foreach (var data in rfqitems)
                    {
                        try
                        {
                            var rfitems = new RFQItems_N()
                            {
                                RFQItemsId = data.RFQItemsId,
                                RFQRevisionId = remoterevisionid,
                                ItemId = data.ItemId,
                                MPRItemDetailsid = data.MPRItemDetailsid,
                                QuotationQty = data.QuotationQty,
                                VendorModelNo = data.VendorModelNo,
                                HSNCode = data.HSNCode,
                                RequestRemarks = data.RequestRemarks,
                                DeleteFlag = false
                            };
                            try
                            {
                                revision.RFQItems_N.Add(rfitems);
                                obj.SaveChanges();
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            List<MPRDocument> mprdocumnts = obj.MPRDocuments.Where(li => li.ItemDetailsId == data.MPRItemDetailsid).ToList();
                            var rfqdocs = vscm.RemoteRFQDocuments.Where(li => li.rfqItemsid == rfitems.RFQItemsId).ToList();
                            foreach (var item in rfqdocs)
                            {
                                RFQDocument rfqDoc = new RFQDocument();
                                rfqDoc.RfqDocId = item.RfqDocId;
                                rfqDoc.rfqRevisionId = item.rfqRevisionId;
                                rfqDoc.rfqItemsid = item.rfqItemsid;
                                rfqDoc.DocumentName = item.DocumentName;
                                rfqDoc.DocumentType = item.DocumentType;
                                rfqDoc.Path = item.Path;
                                rfqDoc.UploadedBy = item.UploadedBy;
                                rfqDoc.UploadedDate = item.uploadedDate;
                                try
                                {
                                    obj.RFQDocuments.Add(rfqDoc);
                                    obj.SaveChanges();
                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                    status.Sid = rfqlocal.RfqMasterId;
                    foreach (RFQTermsModel terms in model.RFQTerms)
                    {
                        try
                        {

                            terms.RFQrevisionId = revisionid;
                            InsertRFQTerms(terms);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }
                return status;
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

        public int addNewRfqRevision(int rfqRevisionId)
        {
            RemoteRFQRevisions_N mprLastRecord = vscm.RemoteRFQRevisions_N.OrderByDescending(p => p.rfqRevisionId).Where(li => li.rfqRevisionId == rfqRevisionId).FirstOrDefault<RemoteRFQRevisions_N>();
            mprLastRecord.ActiveRevision = false;
            mprLastRecord.RevisionNo = mprLastRecord.RevisionNo + 1;
            vscm.SaveChanges();
            RemoteRFQRevisions_N Remtoterevision = vscm.RemoteRFQRevisions_N.Where(li => li.rfqRevisionId == rfqRevisionId).Include(x => x.RemoteRFQItems_N).FirstOrDefault<RemoteRFQRevisions_N>();
            RemoteRFQRevisions_N revision = new RemoteRFQRevisions_N();
            revision.rfqMasterId = Remtoterevision.rfqMasterId;
            revision.RevisionNo = Remtoterevision.RevisionNo;
            revision.ActiveRevision = true;
            revision.CreatedBy = Remtoterevision.CreatedBy;
            revision.CreatedDate = DateTime.Now;
            revision.RFQValidDate = Remtoterevision.RFQValidDate;
            revision.SalesTax = Remtoterevision.SalesTax;
            revision.PaymentTermRemarks = Remtoterevision.PaymentTermRemarks;
            revision.PackingForwarding = Remtoterevision.PackingForwarding;
            revision.PaymentTermDays = Remtoterevision.PaymentTermDays;
            revision.Insurance = Remtoterevision.Insurance;
            revision.ExciseDuty = Remtoterevision.ExciseDuty;
            revision.ShipmentModeid = Remtoterevision.ShipmentModeid;
            revision.BankGuarantee = Remtoterevision.BankGuarantee;
            revision.DeliveryMaxWeeks = Remtoterevision.DeliveryMaxWeeks;
            revision.DeliveryMinWeeks = Remtoterevision.DeliveryMinWeeks;

            vscm.RemoteRFQRevisions_N.Add(revision);
            vscm.SaveChanges();

            var RemoteRFQItems_N = vscm.RemoteRFQItems_N.Where(li => li.RFQRevisionId == rfqRevisionId).ToList();
            foreach (RemoteRFQItems_N rfitems in RemoteRFQItems_N)
            {
                var rfqRemoteitem = new RemoteRFQItems_N();
                rfqRemoteitem.RFQRevisionId = revision.rfqRevisionId;
                rfqRemoteitem.MPRItemDetailsid = rfitems.MPRItemDetailsid;
                rfqRemoteitem.ItemId = rfitems.ItemId;
                rfqRemoteitem.ItemName = rfitems.ItemName;
                rfqRemoteitem.ItemDescription = rfitems.ItemDescription;
                rfqRemoteitem.HSNCode = rfitems.HSNCode;
                rfqRemoteitem.QuotationQty = rfitems.QuotationQty;
                rfqRemoteitem.VendorModelNo = rfitems.VendorModelNo;
                rfqRemoteitem.IGSTPercentage = rfitems.IGSTPercentage;
                rfqRemoteitem.SGSTPercentage = rfitems.SGSTPercentage;
                rfqRemoteitem.CGSTPercentage = rfitems.CGSTPercentage;
                rfqRemoteitem.MfgModelNo = rfitems.MfgModelNo;
                rfqRemoteitem.MfgPartNo = rfitems.MfgPartNo;
                rfqRemoteitem.PFAmount = rfitems.PFAmount;
                rfqRemoteitem.PFPercentage = rfitems.PFPercentage;
                rfqRemoteitem.FreightAmount = rfitems.FreightAmount;
                rfqRemoteitem.CustomDuty = rfitems.CustomDuty;
                rfqRemoteitem.taxInclusiveOfDiscount = rfitems.taxInclusiveOfDiscount;
                vscm.RemoteRFQItems_N.Add(rfqRemoteitem);
                vscm.SaveChanges();

                var remoteRfqdocumnts = vscm.RemoteRFQDocuments.Where(li => li.rfqRevisionId == rfqRevisionId).ToList();

                foreach (var item in remoteRfqdocumnts)
                {

                    RemoteRFQDocument rfqDoc = new RemoteRFQDocument();
                    rfqDoc.rfqRevisionId = revision.rfqRevisionId;
                    rfqDoc.rfqItemsid = item.rfqItemsid;
                    rfqDoc.DocumentName = item.DocumentName;
                    rfqDoc.DocumentType = item.DocumentType;
                    rfqDoc.Path = item.Path;
                    rfqDoc.UploadedBy = item.UploadedBy;
                    rfqDoc.uploadedDate = DateTime.Now;
                    try
                    {
                        vscm.RemoteRFQDocuments.Add(rfqDoc);
                        vscm.SaveChanges();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                var remoteRFQItemInfos = vscm.RemoteRFQItemsInfo_N.Where(li => li.RFQItemsId == rfitems.RFQItemsId).ToList();
                foreach (var remoterfqItemInfo in remoteRFQItemInfos)
                {
                    var remoteinfo = new RemoteRFQItemsInfo_N()
                    {
                        RFQItemsId = rfqRemoteitem.RFQItemsId,
                        Qty = remoterfqItemInfo.Qty,
                        UOM = remoterfqItemInfo.UOM,
                        UnitPrice = remoterfqItemInfo.UnitPrice,
                        DiscountPercentage = remoterfqItemInfo.DiscountPercentage,
                        Discount = remoterfqItemInfo.Discount,
                        CurrencyId = remoterfqItemInfo.CurrencyId,
                        CurrencyValue = remoterfqItemInfo.CurrencyValue,
                        Remarks = remoterfqItemInfo.Remarks,
                        DeliveryDate = remoterfqItemInfo.DeliveryDate,
                        //GSTPercentage = item.GSTPercentage,
                        SyncDate = System.DateTime.Now
                    };
                    vscm.RemoteRFQItemsInfo_N.Add(remoteinfo);
                    vscm.SaveChanges();
                }
            }

            RFQRevisions_N mprLastRecord1 = obj.RFQRevisions_N.OrderByDescending(p => p.rfqRevisionId).Where(li => li.rfqRevisionId == rfqRevisionId).FirstOrDefault<RFQRevisions_N>();
            mprLastRecord1.ActiveRevision = false;
            //mprLastRecord1.RevisionNo = mprLastRecord.RevisionNo + 1;
            obj.SaveChanges();
            RFQRevisions_N Localrevision = new RFQRevisions_N();
            RemoteRFQRevisions_N Localmodel = vscm.RemoteRFQRevisions_N.Where(li => li.rfqRevisionId == revision.rfqRevisionId).FirstOrDefault<RemoteRFQRevisions_N>();
            Localrevision.rfqRevisionId = revision.rfqRevisionId;
            Localrevision.rfqMasterId = revision.rfqMasterId;
            Localrevision.RevisionNo = revision.RevisionNo;
            Localrevision.ActiveRevision =Convert.ToBoolean(revision.ActiveRevision);
            Localrevision.CreatedBy = Localmodel.CreatedBy;
            Localrevision.CreatedDate = DateTime.Now;
            Localrevision.RFQValidDate = Localmodel.RFQValidDate;
            Localrevision.SalesTax = Localmodel.SalesTax;
            Localrevision.PaymentTermRemarks = Localmodel.PaymentTermRemarks;
            Localrevision.PackingForwarding = Localmodel.PackingForwarding;
            Localrevision.PaymentTermDays = Localmodel.PaymentTermDays;
            Localrevision.Insurance = Localmodel.Insurance;
            Localrevision.ExciseDuty = Localmodel.ExciseDuty;
            Localrevision.ShipmentModeid = Localmodel.ShipmentModeid;
            Localrevision.BankGuarantee = Localmodel.BankGuarantee;
            Localrevision.DeliveryMaxWeeks = Localmodel.DeliveryMaxWeeks;
            Localrevision.DeliveryMinWeeks = Localmodel.DeliveryMinWeeks;
            Localrevision.DeleteFlag = false;

            try
            {
                obj.RFQRevisions_N.Add(Localrevision);
                obj.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }

            var localrfqitems = vscm.RemoteRFQItems_N.Where(li => li.RFQRevisionId == Localrevision.rfqRevisionId).ToList();
            foreach (var data in localrfqitems)
            {
                try
                {
                    int rfqitemid = data.RFQItemsId;
                    var localRfitem = new RFQItems_N()
                    {
                        RFQItemsId = data.RFQItemsId,
                        RFQRevisionId = Localrevision.rfqRevisionId,
                        ItemId = data.ItemId,
                        MPRItemDetailsid = data.MPRItemDetailsid,
                        QuotationQty = data.QuotationQty,
                        VendorModelNo = data.VendorModelNo,
                        HSNCode = data.HSNCode,
                        RequestRemarks = data.RequestRemarks,
                        DeleteFlag = false
                    };
                    try
                    {
                        Localrevision.RFQItems_N.Add(localRfitem);
                        obj.SaveChanges();
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    var rfqdocs = vscm.RemoteRFQDocuments.Where(li => li.rfqRevisionId == revision.rfqRevisionId).ToList();
                    foreach (var item in rfqdocs)
                    {
                        var localdoc = obj.RFQDocuments.Where(li => li.RfqDocId == item.RfqDocId).FirstOrDefault();
                        if (localdoc == null)
                        {
                            RFQDocument rfqDoc = new RFQDocument();
                            rfqDoc.RfqDocId = item.RfqDocId;
                            rfqDoc.rfqRevisionId = item.rfqRevisionId;
                            rfqDoc.rfqItemsid = localRfitem.RFQItemsId;
                            rfqDoc.DocumentName = item.DocumentName;
                            rfqDoc.DocumentType = item.DocumentType;
                            rfqDoc.Path = item.Path;
                            rfqDoc.UploadedBy = item.UploadedBy;
                            rfqDoc.UploadedDate = item.uploadedDate;
                            try
                            {
                                obj.RFQDocuments.Add(rfqDoc);
                                obj.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }
                        }
                    }
                    var localRfqItemInfos = vscm.RemoteRFQItemsInfo_N.Where(li => li.RFQItemsId == rfqitemid).ToList();
                    foreach (var localRfqItemInfo in localRfqItemInfos)
                    {
                        var info = new RFQItemsInfo_N()
                        {
                            RFQSplitItemId = localRfqItemInfo.RFQSplitItemId,
                            RFQItemsId = localRfitem.RFQItemsId,
                            Qty = localRfqItemInfo.Qty,
                            UOM = localRfqItemInfo.UOM,
                            UnitPrice = localRfqItemInfo.UnitPrice,
                            DiscountPercentage = localRfqItemInfo.DiscountPercentage,
                            Discount = localRfqItemInfo.Discount,
                            CurrencyId = localRfqItemInfo.CurrencyId,
                            CurrencyValue = localRfqItemInfo.CurrencyValue,
                            Remarks = localRfqItemInfo.Remarks,
                            DeliveryDate = localRfqItemInfo.DeliveryDate
                        };
                        obj.RFQItemsInfo_N.Add(info);
                        obj.SaveChanges();
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            var RFQTerms = vscm.RemoteRfqTerms.Where(li => li.RfqRevisionId == rfqRevisionId).ToList();
            foreach (var data in RFQTerms)
            {
                RFQTermsModel rfqterm = new RFQTermsModel();
                rfqterm.termsid = data.termsid;
                rfqterm.RFQrevisionId = Localrevision.rfqRevisionId;
                rfqterm.TermGroup = data.TermGroup;
                rfqterm.Terms = data.Terms;
                rfqterm.CreatedBy = data.CreatedBy;
                rfqterm.CreatedDate = DateTime.Now;
                InsertRFQTerms(rfqterm);
            }


            return revision.rfqRevisionId;
        }


        public async Task<statuscheckmodel> CreateNewRfq(RFQMasterModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var rfqremote = new RemoteRFQMaster();
                if (model != null)
                {
                    //var remoteconnection = obj.Database.Connection.ConnectionString;
                    var remote = vscm.Database.Connection.ConnectionString;
                    vscm.Database.Connection.Open();
                    //vscm.Database.Connection.Close();
                    var result = obj.Database.Connection.ConnectionString;

                    rfqremote.RFQNo = model.RfqNo;
                    rfqremote.RFQUniqueNo = model.RfqUniqueNo;
                    rfqremote.MPRRevisionId = model.MPRRevisionId;
                    rfqremote.VendorId = model.VendorId;
                    rfqremote.CreatedBy = model.CreatedBy;
                    rfqremote.CreatedDate = model.Created;
                    vscm.RemoteRFQMasters.Add(rfqremote);
                    vscm.SaveChanges();
                }
                int masterid = rfqremote.RfqMasterId;
                vscm.Database.Connection.Close();
                var rfqlocal = new RFQMaster();
                if (model != null)
                {
                    var localconnection = vscm.Database.Connection.ConnectionString;
                    obj.Database.Connection.Open();
                    rfqlocal.RfqMasterId = masterid;
                    rfqlocal.RFQNo = model.RfqNo;
                    rfqlocal.RFQUniqueNo = model.RfqUniqueNo;
                    rfqlocal.MPRRevisionId = model.MPRRevisionId;
                    rfqlocal.VendorId = model.VendorId;
                    rfqlocal.CreatedBy = model.CreatedBy;
                    rfqlocal.CreatedDate = model.Created;
                    obj.RFQMasters.Add(rfqlocal);
                    obj.SaveChanges();
                    obj.Database.Connection.Close();
                }

                return status;
            }

            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<List<RFQMasterModel>> getallrfqlist()
        {
            List<RFQMasterModel> master = new List<RFQMasterModel>();
            try
            {
                var result = obj.RFQMasters.Where(x => x.DeleteFlag == false).Include(x => x.RFQRevisions_N).ToList();
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        master = result.ConvertAll(x => new RFQMasterModel()
                        {
                            RfqMasterId = x.RfqMasterId,
                            VendorId = x.VendorId,
                            RfqNo = x.RFQNo,
                            RfqUniqueNo = x.RFQUniqueNo,
                            Created = x.CreatedDate,
                            CreatedBy = x.CreatedBy,
                            Revision = x.RFQRevisions_N.Where(y => y.rfqMasterId == x.RfqMasterId).Select(y => new RfqRevisionModel()
                            {
                                RfqRevisionId = y.rfqRevisionId,
                                RfqValidDate = y.RFQValidDate,
                                RfqMasterId = y.rfqMasterId,
                                RfqRevisionNo = y.RevisionNo
                            }).ToList()
                        });

                    }

                }
                //remote data
                //var remotedata = vscm.RemoteRFQMasters.Where(x => x.DeleteFlag == false).Include(x => x.RemoteRFQRevisions).ToList();
                //if (remotedata != null)
                //{
                //    foreach (var item in remotedata)
                //    {
                //        master = result.ConvertAll(x => new RFQMasterModel()
                //        {
                //            RfqMasterId = x.RfqMasterId,
                //            VendorId = x.VendorId,
                //            RfqNo = x.RFQNo,
                //            RfqUniqueNo = x.RFQUniqueNo,
                //            Created = x.CreatedDate,
                //            CreatedBy = x.CreatedBy,
                //            Revision = x.RFQRevisions.Where(y => y.rfqMasterId == x.RfqMasterId).Select(y => new RfqRevisionModel()
                //            {
                //                RfqRevisionId = y.rfqRevisionId,
                //                RfqValidDate = y.RFQValidDate,
                //                RfqMasterId = y.rfqMasterId,
                //                RfqRevisionNo = y.RevisionNo
                //            }).ToList()
                //        });

                //    }

                //}
                return master;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //public async Task<List<RfqRevisionModel>> GetAllRFQs()
        //{
        //    List<RfqRevisionModel> model;

        //    try
        //    {
        //        obj.Configuration.ProxyCreationEnabled = false;
        //        model = obj.RFQRevisions.Where(x => x.DeleteFlag == false).Include(x => x.RFQMaster).ToList();
        //        //foreach (var item in revision)
        //        //{
        //        //    model = revision.Select(x => new RfqRevisionModel()
        //        //    {
        //        //        RfqRevisionId = x.rfqRevisionId,
        //        //        RfqRevisionNo = x.RevisionNo,
        //        //        RfqValidDate = x.RFQValidDate,
        //        //        CreatedBy = x.CreatedBy,
        //        //        CreatedDate = x.CreatedDate,
        //        //    }).ToList();
        //        //    foreach (var items in revision)
        //        //    {
        //        //        foreach (var masters in revision)
        //        //        {

        //        //        }
        //        //    }

        //        //}   




        //        //foreach (var item in revision)
        //        //{
        //        //    revision.Select(x => new RfqRevisionModel()
        //        //    {
        //        //        RfqRevisionId = item.rfqRevisionId,
        //        //        CreatedDate = item.CreatedDate,
        //        //        RfqValidDate = item.RFQValidDate,
        //        //    }).ToList();
        //        //    revision.Select(x => new RFQMasterModel()
        //        //    {
        //        //        RfqMasterId=item.RFQMaster.RfqMasterId,
        //        //        RfqUniqueNo=item.RFQMaster.RFQUniqueNo,
        //        //        RfqNo=item.RFQMaster.RFQNo
        //        //    }).ToList();

        //        //}
        //        return model;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //public async Task<RFQMasterDataModel> GetAllRFQs()
        //{
        //    RFQMasterDataModel rfqitems = new RFQMasterDataModel();
        //    //var rfqs = from x in obj.RFQMasters where x.DeleteFlag=="false"  select x;
        //    try
        //    {
        //        var rfqs = obj.RFQMasters.Where(x => x.DeleteFlag == false).Select(x => x).ToList();
        //        //var rfqs = obj.RFQMasters.SqlQuery("select * from RFQMaster where DeleteFlag=0");
        //        // var rfqs = obj.RFQMasters.Where(x => x.DeleteFlag = false).ToList();
        //        //List<RFQMasterModel> rfqs = (from x in obj.RFQMasters where x.DeleteFlag == false select x)
        //        RFQMasterModel model = new RFQMasterModel();
        //        foreach (var item in rfqs)
        //        {
        //            model.Created = item.CreatedDate;
        //            model.CreatedBy = item.CreatedBy;
        //            model.RfqMasterId = item.RfqMasterId;
        //            model.RfqNo = item.RFQNo;
        //            model.RfqUniqueNo = item.RFQUniqueNo;
        //            model.VendorId = item.VendorId;
        //            rfqitems.RFQlist.Add(model);
        //        }
        //        return rfqitems;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}
        ////public async Task<RFQMasterDataModel> GetAllRFQs()
        //{
        //    RFQMasterDataModel rfqitems = new RFQMasterDataModel();
        //    List< RFQMasterModel> rfqs = new List< RFQMasterModel>();
        //    //var rfqs = from x in obj.RFQMasters where x.DeleteFlag=="false"  select x;
        //    try
        //    {
        //         rfqs = (from x in obj.RFQMasters
        //                    where x.DeleteFlag == false
        //                    select new RFQMasterModel
        //                    {
        //                       RfqMasterId=x.RfqMasterId,
        //                       RfqUniqueNo=x.RFQUniqueNo,
        //                       RfqNo=x.RFQNo,
        //                       VendorId=x.VendorId,
        //                       CreatedBy=x.CreatedBy,
        //                       Created=x.CreatedDate
        //                    }).ToList();
        //        rfqitems.RFQlist.Add(rfqs);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}
        public async Task<List<RfqRevisionModel>> GetAllrevisionRFQs()
        {
            List<RfqRevisionModel> rfq = new List<RfqRevisionModel>();
            try
            {
                return (from x in obj.RFQRevisions
                        where x.DeleteFlag == false
                        select new RfqRevisionModel()
                        {
                            RfqRevisionId = x.rfqRevisionId,
                            RfqMasterId = x.rfqMasterId,
                            CreatedBy = x.CreatedBy,
                            CreatedDate = x.CreatedDate,
                            RfqRevisionNo = x.RevisionNo,
                            RfqValidDate = x.RFQValidDate
                        }).ToList();

            }

            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<RFQMasterModel> GetRFQById(int masterID)
        {
            RFQMasterModel model = new RFQMasterModel();
            RFQMaster rfq = new RFQMaster();
            RFQItem rfqitem = new RFQItem();
            try
            {
                rfq = obj.RFQMasters.Where(x => x.RfqMasterId == masterID).Include(x => x.RFQRevisions_N).SingleOrDefault<RFQMaster>();
                //rfq.RFQRevisions = obj.RFQRevisions.Where(x => x.rfqMasterId == id).Include(x=>x.RFQItems).ToList<RFQRevision>();
                if (rfq != null)
                {
                    model.RfqMasterId = rfq.RfqMasterId;
                    model.RfqNo = rfq.RFQNo;
                    model.RfqUniqueNo = rfq.RFQUniqueNo;
                    model.VendorId = rfq.VendorId;
                    model.CreatedBy = rfq.CreatedBy;


                    foreach (var item in rfq.RFQRevisions_N)
                    {
                        RfqRevisionModel RfqRevisionModel = new RfqRevisionModel();
                        RfqRevisionModel.RfqMasterId = item.rfqMasterId;
                        RfqRevisionModel.RfqRevisionId = item.rfqRevisionId;
                        RfqRevisionModel.RfqValidDate = item.RFQValidDate;
                        RfqRevisionModel.CreatedBy = item.CreatedBy;
                        RfqRevisionModel.CreatedDate = item.CreatedDate;
                        RfqRevisionModel.salesTax = item.SalesTax;
                        RfqRevisionModel.PaymentTermDays = item.PaymentTermDays;
                        RfqRevisionModel.freight = item.Freight;
                        RfqRevisionModel.Insurance = item.Insurance;
                        RfqRevisionModel.IsDeleted = item.DeleteFlag;
                        RfqRevisionModel.DeliveryMaxWeeks = item.DeliveryMaxWeeks;
                        RfqRevisionModel.DeliveryMinWeeks = item.DeliveryMinWeeks;
                        RfqRevisionModel.CustomsDuty = item.CustomsDuty;
                        RfqRevisionModel.BankGuarantee = item.BankGuarantee;
                        RfqRevisionModel.ExciseDuty = item.ExciseDuty;

                        model.Revision.Add(RfqRevisionModel);


                        //var result = obj.RFQItems.Where(x => x.RFQRevisionId == RfqRevisionModel.RfqRevisionId).ToList<RFQItem>();
                        //if (result != null)
                        //{
                        //    RfqItemModel rfqs = new RfqItemModel();
                        //    foreach (RFQItem items in item.RFQItems)
                        //    {
                        //        rfqs.RFQItemID = items.RFQItemsId;
                        //        rfqs.HSNCode = items.HSNCode;
                        //        rfqs.RFQRevisionId = items.RFQRevisionId;
                        //        rfqs.MRPItemsDetailsID = items.MPRItemDetailsid;
                        //        rfqs.QuotationQty = items.QuotationQty;
                        //        rfqs.VendorModelNo = items.VendorModelNo;
                        //        rfqs.RequsetRemarks = items.RequestRemarks;

                        //        RfqRevisionModel.rfqitem.Add(rfqs);
                        //    }

                        //}

                    }
                }
                //var gets = from x in obj.RFQMasters where x.RfqMasterId == id select x;

                return model;
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
        public statuscheckmodel DeleteRfqById(int rfqmasterid)
        {
            statuscheckmodel status = new statuscheckmodel();
            var domainentity = obj.Set<RFQMaster>().Where(x => x.RfqMasterId == rfqmasterid && x.DeleteFlag == false).Include(x => x.RFQRevisions_N).SingleOrDefault();

            if (domainentity != null)
            {
                domainentity.DeleteFlag = true;
                obj.SaveChanges();
                var revision = obj.RFQRevisions.Where(l => l.rfqMasterId == domainentity.RfqMasterId && l.DeleteFlag == false).ToList();
                if (revision != null)
                {
                    foreach (var item in revision)
                    {
                        item.DeleteFlag = true;
                        obj.SaveChanges();
                    }

                    var ids = revision.Select(x => x.rfqRevisionId).ToList();
                    var itemlist = obj.RFQItems.Where(x => ids.Contains(x.RFQRevisionId) && x.DeleteFlag == false).ToList();
                    if (itemlist != null)
                    {
                        foreach (var item in itemlist)
                        {
                            item.DeleteFlag = true;
                            obj.SaveChanges();
                        }
                    }
                }
            }
            return status;
        }

        //public async Task<statusmodel> UpdateRfqRevision(RFQMasterModel model)
        //{
        //    statusmodel status = new statusmodel();
        //    var result = obj.RFQMasters.Where(x => x.RfqMasterId == model.RfqMasterId).Include(x=>x.RFQRevisions).FirstOrDefault<RFQMaster>();
        //    try
        //    {
        //        if (result!=null)
        //        {
        //            foreach (var item in model.Revision)
        //            {
        //                // var revision = new RFQRevision();
        //                var existing = result.RFQRevisions.Where(x => x.rfqMasterId == item.RfqMasterId).ToList<RFQRevision>();
        //                if (existing!=null)
        //                {
        //                    obj.Entry(existing).CurrentValues.SetValues(item);
        //                }

        //                //revision.SalesTax = item.salesTax;
        //                //revision.RFQValidDate = item.RfqValidDate;
        //                //revision.CreatedBy = item.CreatedBy;
        //                //revision.PaymentTermDays = item.PaymentTermDays;
        //                //revision.DeliveryMaxWeeks = item.DeliveryMaxWeeks;
        //                //revision.DeliveryMinWeeks = item.DeliveryMinWeeks;
        //                //revision.BankGuarantee = item.BankGuarantee;
        //                //revision.ExciseDuty = item.ExciseDuty;
        //                //revision.CustomsDuty = item.CustomsDuty;
        //                //revision.Insurance = item.Insurance;
        //                //revision.Freight = item.freight;
        //                //revision.PackingForwarding = item.PackingForwading;
        //                //revision.CreatedDate = item.CreatedDate;
        //            }

        //            await obj.SaveChangesAsync();
        //        }
        //        status.Sid = model.RfqMasterId;
        //        return  status;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}
        public async Task<statuscheckmodel> UpdateRfqRevision(RfqRevisionModel model)
        {
            statuscheckmodel status = new statuscheckmodel();

            try
            {
                vscm.Database.Connection.Open();
                var remotedata = vscm.RemoteRFQRevisions_N.Where(x => x.rfqRevisionId == model.RfqRevisionId).Include(x => x.RemoteRFQMaster).FirstOrDefault<RemoteRFQRevisions_N>();
                if (remotedata != null)
                {
                    RemoteRFQRevision revision = new RemoteRFQRevision();
                    remotedata.SalesTax = model.salesTax;
                    remotedata.RFQValidDate = model.RfqValidDate;
                    remotedata.CreatedBy = model.CreatedBy;
                    remotedata.PaymentTermDays = model.PaymentTermDays;
                    remotedata.DeliveryMaxWeeks = model.DeliveryMaxWeeks;
                    remotedata.DeliveryMinWeeks = model.DeliveryMinWeeks;
                    remotedata.BankGuarantee = model.BankGuarantee;
                    remotedata.ExciseDuty = model.ExciseDuty;
                    remotedata.CustomsDuty = model.CustomsDuty;
                    remotedata.Insurance = model.Insurance;
                    remotedata.Freight = model.freight;
                    remotedata.PackingForwarding = model.PackingForwading;
                    remotedata.CreatedDate = model.CreatedDate;
                    vscm.SaveChanges();
                }
                vscm.Database.Connection.Close();
                obj.Database.Connection.Open();
                var result = obj.RFQRevisions_N.Where(x => x.rfqRevisionId == model.RfqRevisionId).Include(x => x.RFQMaster).FirstOrDefault<RFQRevisions_N>();
                if (result != null)
                {
                    RFQRevision revision = new RFQRevision();
                    result.SalesTax = model.salesTax;
                    result.RFQValidDate = model.RfqValidDate;
                    result.CreatedBy = model.CreatedBy;
                    result.PaymentTermDays = model.PaymentTermDays;
                    result.DeliveryMaxWeeks = model.DeliveryMaxWeeks;
                    result.DeliveryMinWeeks = model.DeliveryMinWeeks;
                    result.BankGuarantee = model.BankGuarantee;
                    result.ExciseDuty = model.ExciseDuty;
                    result.CustomsDuty = model.CustomsDuty;
                    result.Insurance = model.Insurance;
                    result.Freight = model.freight;
                    result.PackingForwarding = model.PackingForwading;
                    result.CreatedDate = model.CreatedDate;

                    obj.SaveChanges();

                }

                status.Sid = model.RfqMasterId;
                return status;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<statuscheckmodel> UpdateBulkRfqRevision(RfqRevisionModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            var result = obj.RFQRevisions_N.Where(x => x.rfqMasterId == model.RfqMasterId).Include(x => x.RFQMaster).ToList<RFQRevisions_N>();
            try
            {

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        item.SalesTax = model.salesTax;
                        item.RFQValidDate = model.RfqValidDate;
                        item.CreatedBy = model.CreatedBy;
                        item.PaymentTermDays = model.PaymentTermDays;
                        item.DeliveryMaxWeeks = model.DeliveryMaxWeeks;
                        item.DeliveryMinWeeks = model.DeliveryMinWeeks;
                        item.BankGuarantee = model.BankGuarantee;
                        item.ExciseDuty = model.ExciseDuty;
                        item.CustomsDuty = model.CustomsDuty;
                        item.Insurance = model.Insurance;
                        item.Freight = model.freight;
                        item.PackingForwarding = model.PackingForwading;
                        item.CreatedDate = model.CreatedDate;

                        obj.SaveChanges();
                    }

                }
                status.Sid = model.RfqMasterId;
                return status;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<statuscheckmodel> UpdateRfqItemByBulk(RfqItemModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                vscm.Database.Connection.Open();
                var Remoteresult = vscm.RemoteRFQItems_N.Where(x => x.RFQRevisionId == model.RFQRevisionId).Include(x => x.RemoteRFQRevisions_N).ToList<RemoteRFQItems_N>();

                if (Remoteresult != null)
                {
                    foreach (var item in Remoteresult)
                    {
                        item.HSNCode = model.HSNCode;
                        item.QuotationQty = model.QuotationQty;
                        item.VendorModelNo = model.VendorModelNo;
                        item.RequestRemarks = model.RequsetRemarks;
                        item.CGSTPercentage = model.CGSTPercentage;
                        item.SGSTPercentage = model.SGSTPercentage;
                        item.IGSTPercentage = model.IGSTPercentage;
                        item.FreightAmount = model.FreightAmount;
                        item.FreightPercentage = model.FreightPercentage;
                        item.PFAmount = model.PFAmount;
                        item.PFPercentage = model.PFPercentage;
                        item.RequestRemarks = model.RequsetRemarks;
                        obj.SaveChanges();
                    }
                }
                vscm.Database.Connection.Close();

                obj.Database.Connection.Open();

                var result = obj.RFQItems.Where(x => x.RFQRevisionId == model.RFQRevisionId).Include(x => x.RFQRevision).ToList<RFQItem>();

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        item.HSNCode = model.HSNCode;
                        item.QuotationQty = model.QuotationQty;
                        item.VendorModelNo = model.VendorModelNo;
                        item.RequestRemarks = model.RequsetRemarks;
                        item.CGSTPercentage = model.CGSTPercentage;
                        item.SGSTPercentage = model.SGSTPercentage;
                        item.IGSTPercentage = model.IGSTPercentage;
                        item.FreightAmount = model.FreightAmount;
                        item.FreightPercentage = model.FreightPercentage;
                        item.PFAmount = model.PFAmount;
                        item.PFPercentage = model.PFPercentage;
                        item.RequestRemarks = model.RequsetRemarks;
                        obj.SaveChanges();
                    }
                }
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<RfqItemModel>> GetItemsByRevisionId(int revisionid)
        {
            List<RfqItemModel> rfq = new List<RfqItemModel>();
            try
            {
                var data = vscm.RemoteRFQItems_N.Where(x => x.RFQRevisionId == revisionid).ToList();
                var itemid = data.Select(x => new RfqItemModel()
                {
                    RFQItemID = x.RFQItemsId
                }).FirstOrDefault();
                var iteminfodata = vscm.RemoteRFQItemsInfo_N.ToList();

                foreach (var item in data)
                {
                    rfq.Add(new RfqItemModel()
                    {
                        RFQItemID = item.RFQItemsId,
                        HSNCode = item.HSNCode,
                        QuotationQty = Convert.ToDouble(item.QuotationQty),
                        VendorModelNo = item.VendorModelNo,
                        CustomDuty = Convert.ToDecimal(item.CustomDuty),
                        RequsetRemarks = item.RequestRemarks,
                        iteminfo = iteminfodata.Where(x => x.RFQItemsId == item.RFQItemsId && x.DeleteFlag == false).Select(x => new RfqItemInfoModel()
                        {
                            RFQSplitItemId = x.RFQSplitItemId,
                            Qunatity = x.Qty,
                            UOM = x.UOM,
                            UnitPrice = x.UnitPrice,
                            DiscountPercentage = x.DiscountPercentage,
                            Discount = x.Discount,
                            CurrencyID = x.CurrencyId,
                            CurrencyValue = x.CurrencyValue,
                            Remarks = x.Remarks
                        }).ToList()
                    });

                }


                return rfq;
            }

            catch (Exception ex)
            {
                throw;
            }
        }

        //public async Task<List<RfqItemModel>> GetItemsByRevisionId(int revisionid)
        //{
        //    List<RfqItemModel> rfq = new List<RfqItemModel>();
        //    try
        //    {
        //        return (from x in vscm.RemoteRFQItems
        //                where x.RFQRevisionId == revisionid
        //                select new RfqItemModel
        //                {
        //                    HSNCode = x.HSNCode,
        //                    QuotationQty = x.QuotationQty,
        //                    RFQRevisionId = x.RFQRevisionId,
        //                    VendorModelNo = x.VendorModelNo,
        //                    RequsetRemarks = x.RequestRemarks,
        //                    RFQItemID = x.RFQItemsId,
        //                    ItemName = x.ItemName,
        //                    ItemDescription = x.ItemDescription
        //                }).ToList();

        //    }

        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        public async Task<List<RfqItemModel>> GetRfqItemsByRevisionId(int revisionid)
        {
            List<RfqItemModel> rfq = new List<RfqItemModel>();
            try
            {
                var itemdata = vscm.RemoteRFQItems_N.Where(x => x.RFQRevisionId == revisionid).ToList();
                if (itemdata != null)
                {
                    rfq = itemdata.Select(x => new RfqItemModel()
                    {
                        HSNCode = x.HSNCode,
                        QuotationQty = Convert.ToDouble(x.QuotationQty),
                        RFQRevisionId = x.RFQRevisionId,
                        VendorModelNo = x.VendorModelNo,
                        RFQItemID = x.RFQItemsId,
                        RequsetRemarks = x.RequestRemarks,
                        ItemName = x.ItemName,
                        ItemDescription = x.ItemDescription,
                    }).ToList();
                    var data = itemdata.Select(x => new RfqItemModel()
                    {
                        RFQRevisionId = x.RFQRevisionId
                    }).FirstOrDefault();
                    RfqItemModel model = new RfqItemModel();
                    var termsdata = vscm.RemoteRfqTerms.Where(x => x.RfqRevisionId == data.RFQRevisionId).ToList();
                    rfq.ConvertAll(x => new RfqItemModel()
                    {
                        rfqterms = termsdata.Select(y => new RFQTermsModel()
                        {
                            RfqTermsid = y.VRfqTermsid,
                            Terms = y.Terms,
                            TermGroup = y.TermGroup,
                            termsid = y.termsid,
                            SyncStatus = y.SyncStatus
                        }).ToList()
                    }).ToList();

                }
                return rfq;

            }

            catch (Exception ex)
            {
                throw;
            }
        }
        //public RFQMasterModel getItemsbyRevisionno(string id)
        // {
        //     RFQMasterModel master = new RFQMasterModel();
        //     try
        //     {
        //         var masters = obj.RFQMasters.Where(x => x.RFQNo == id && x.DeleteFlag == false).Include(x => x.RFQRevisions_N).Select(x => x).SingleOrDefault();
        //         if (masters!=null)
        //         {

        //         }
        //     }
        //     catch (Exception)
        //     {

        //         throw;
        //     }
        // }
        public async Task<statuscheckmodel> UpdateSingleRfqItem(RfqItemModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                vscm.Database.Connection.Open();
                var remotedata = vscm.RemoteRFQItems_N.Where(x => x.RFQItemsId == model.RFQItemID).Include(x => x.RemoteRFQRevisions_N).FirstOrDefault<RemoteRFQItems_N>();
                if (remotedata != null)
                {
                    remotedata.HSNCode = model.HSNCode;
                    remotedata.QuotationQty = model.QuotationQty;
                    remotedata.VendorModelNo = model.VendorModelNo;
                    remotedata.RequestRemarks = model.RequsetRemarks;
                    remotedata.CGSTPercentage = model.CGSTPercentage;
                    remotedata.SGSTPercentage = model.SGSTPercentage;
                    remotedata.IGSTPercentage = model.IGSTPercentage;
                    remotedata.FreightAmount = model.FreightAmount;
                    remotedata.FreightPercentage = model.FreightPercentage;
                    remotedata.PFAmount = model.PFAmount;
                    remotedata.PFPercentage = model.PFPercentage;
                    remotedata.RequestRemarks = model.RequsetRemarks;
                    vscm.SaveChanges();
                }
                vscm.Database.Connection.Close();

                obj.Database.Connection.Open();
                var data = obj.RFQItems.Where(x => x.RFQItemsId == model.RFQItemID).Include(x => x.RFQRevision).FirstOrDefault<RFQItem>();
                if (data != null)
                {
                    data.HSNCode = model.HSNCode;
                    data.QuotationQty = model.QuotationQty;
                    data.VendorModelNo = model.VendorModelNo;
                    data.RequestRemarks = model.RequsetRemarks;
                    data.CGSTPercentage = model.CGSTPercentage;
                    data.SGSTPercentage = model.SGSTPercentage;
                    data.IGSTPercentage = model.IGSTPercentage;
                    data.FreightAmount = model.FreightAmount;
                    data.FreightPercentage = model.FreightPercentage;
                    data.PFAmount = model.PFAmount;
                    data.PFPercentage = model.PFPercentage;
                    data.RequestRemarks = model.RequsetRemarks;
                    obj.SaveChanges();
                }
                int id = remotedata.RFQItemsId;
                status.Sid = id;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public statuscheckmodel DeleteRfqRevisionbyId(int id)
        {
            statuscheckmodel staus = new statuscheckmodel();
            try
            {
                var data = obj.RFQRevisions_N.Where(x => x.rfqRevisionId == id && x.DeleteFlag == false).Select(x => x).SingleOrDefault();
                if (data != null)
                {
                    data.DeleteFlag = true;
                    obj.SaveChanges();
                }
                return staus;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public statuscheckmodel DeleteRfqItemById(int id)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = obj.RFQItems.Where(x => x.RFQRevisionId == id && x.DeleteFlag == false).Select(x => x).SingleOrDefault();
                if (data != null)
                {
                    data.DeleteFlag = true;
                    obj.SaveChanges();
                }
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public statuscheckmodel DeleteBulkItemsByItemId(List<int> id)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = obj.RFQItems.Where(x => id.Contains(x.RFQItemsId)).ToList();
                if (data != null)
                {
                    data.Select(x => x.DeleteFlag == true);
                    obj.SaveChanges();
                }
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> InsertDocument(RfqDocumentsModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                RemoteRFQDocument remote = new RemoteRFQDocument();
                vscm.Database.Connection.Open();
                remote.DocumentName = model.DocumentName;
                remote.DocumentType = model.DocumentType;
                remote.Path = model.Path;
                remote.UploadedBy = model.UploadedBy;
                remote.uploadedDate = model.UploadedDate;
                remote.Status = model.Status;
                remote.StatusBy = model.StatusBy;
                remote.StatusDate = model.Statusdate;
                vscm.RemoteRFQDocuments.Add(remote);
                vscm.SaveChanges();
                status.Sid = remote.RfqDocId;
                vscm.Database.Connection.Close();


                obj.Database.Connection.Open();
                RFQDocument newObj = new RFQDocument();
                newObj.RfqDocId = status.Sid;
                newObj.DocumentName = model.DocumentName;
                newObj.DocumentType = model.DocumentType;
                newObj.Path = model.Path;
                newObj.UploadedBy = model.UploadedBy;
                newObj.UploadedDate = model.UploadedDate;
                newObj.Status = model.Status;
                newObj.StatusBy = model.StatusBy;
                newObj.StatusDate = model.Statusdate;
                obj.RFQDocuments.Add(newObj);
                obj.SaveChanges();
                obj.Database.Connection.Close();
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        private string SaveImage(string base64, string FilePath, string ImageName)
        {
            //Get the file type to save in
            var FilePathWithExtension = "";
            string localBase64 = "";

            if (base64.Contains("data:image/jpeg;base64,"))
            {
                FilePathWithExtension = FilePath + ImageName + ".jpg";
                localBase64 = base64.Replace("data:image/jpeg;base64,", "");
            }
            else if (base64.Contains("data:image/png;base64,"))
            {
                FilePathWithExtension = FilePath + ImageName + ".png";
                localBase64 = base64.Replace("data:image/png;base64,", "");
            }
            else if (base64.Contains("data:image/bmp;base64"))
            {
                FilePathWithExtension = FilePath + ImageName + ".bmp";
                localBase64 = base64.Replace("data:image/bmp;base64", "");
            }
            else if (base64.Contains("data:application/msword;base64,"))
            {
                FilePathWithExtension = FilePath + ImageName + ".doc";
                localBase64 = base64.Replace("data:application/msword;base64,", "");
            }
            else if (base64.Contains("data:application/vnd.openxmlformats-officedocument.wordprocessingml.document;base64,"))
            {
                FilePathWithExtension = FilePath + ImageName + ".docx";
                localBase64 = base64.Replace("data:application/vnd.openxmlformats-officedocument.wordprocessingml.document;base64,", "");
            }
            else if (base64.Contains("data:application/pdf;base64,"))
            {
                FilePathWithExtension = FilePath + ImageName + ".pdf";
                localBase64 = base64.Replace("data:application/pdf;base64,", "");
            }

            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(localBase64)))
            {
                using (FileStream fs = new FileStream(FilePathWithExtension, FileMode.Create, FileAccess.Write))
                {
                    //Create the specified directory if it does not exist
                    var photofolder = System.IO.Path.GetDirectoryName(FilePathWithExtension);
                    if (!Directory.Exists(photofolder))
                    {
                        Directory.CreateDirectory(photofolder);
                    }

                    ms.WriteTo(fs);
                    fs.Close();
                    ms.Close();
                }
            }

            return FilePathWithExtension;
        }
        public statuscheckmodel CommunicationAdd(RfqCommunicationModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            var revision = obj.RFQRevisions_N.Where(x => x.rfqRevisionId == model.RfqRevision.RfqRevisionId && x.DeleteFlag == false).Include(x => x.RFQItems_N).SingleOrDefault();
            var item = revision.RFQItems_N.Where(x => x.RFQItemsId == model.RfqItem.RFQItemID && x.DeleteFlag == false).Select(x => x);
            // var result=from x in obj.MPRRevisions where x.RevisionId==model.re
            if (item != null)
            {
                RFQCommunication communication = new RFQCommunication();
                communication.RemarksFrom = model.RemarksFrom;
                communication.RemarksTo = model.RemarksTo;
                communication.ReminderDate = model.ReminderDate;
                communication.SendEmail = model.SendEmail;
                communication.SetReminder = model.SetReminder;
                communication.RemarksDate = model.RemarksDate;
                communication.Remarks = model.Remarks;
                communication.RfqItemsId = model.RfqItem.RFQItemID;
                communication.RfqRevisionId = model.RfqRevision.RfqRevisionId;

                obj.RFQCommunications.Add(communication);
                obj.SaveChanges();
            }
            status.Sid = model.Rfqccid;
            //status.StatusMesssage = model.StatusMesssage;
            return status;
        }
        public async Task<statuscheckmodel> InsertCommunicationAgainstRevision(RfqCommunicationModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                vscm.Database.Connection.Open();
                RemoteRFQCommunication remotecomm = new RemoteRFQCommunication();
                if (model != null)
                {
                    remotecomm.RfqRevisionId = model.RfqRevisionId;
                    remotecomm.RemarksFrom = model.RemarksFrom;
                    remotecomm.RemarksTo = model.RemarksTo;
                    remotecomm.SendEmail = model.SendEmail;
                    remotecomm.SetReminder = model.SetReminder;
                    remotecomm.ReminderDate = model.ReminderDate;
                    remotecomm.RemarksDate = model.RemarksDate;
                    remotecomm.Remarks = model.Remarks;
                    vscm.RemoteRFQCommunications.Add(remotecomm);
                    vscm.SaveChanges();
                }
                int cid = remotecomm.RfqCCid;
                vscm.Database.Connection.Close();

                RFQCommunication localcomm = new RFQCommunication();
                obj.Database.Connection.Open();
                if (model != null)
                {
                    localcomm.RfqCCid = cid;
                    localcomm.RfqRevisionId = model.RfqRevisionId;
                    localcomm.RemarksFrom = model.RemarksFrom;
                    localcomm.RemarksTo = model.RemarksTo;
                    localcomm.SendEmail = model.SendEmail;
                    localcomm.SetReminder = model.SetReminder;
                    localcomm.ReminderDate = model.ReminderDate;
                    localcomm.RemarksDate = model.RemarksDate;
                    localcomm.DeleteFlag = false;
                    localcomm.Remarks = model.Remarks;
                    obj.RFQCommunications.Add(localcomm);
                    obj.SaveChanges();
                }
                obj.Database.Connection.Close();
                status.Sid = cid;
                return status;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<RfqCommunicationModel> GetCommunicationByItemID(int itemid)
        {
            RfqCommunicationModel communication = new RfqCommunicationModel();
            try
            {
                var Remotecommunication = vscm.RemoteRFQCommunications.Where(x => x.RfqItemsId == itemid && x.DeleteFlag == false).FirstOrDefault();
                if (Remotecommunication != null)
                {
                    communication.Rfqccid = Remotecommunication.RfqCCid;
                    communication.RfqRevisionId = Convert.ToInt32(Remotecommunication.RfqRevisionId);
                    communication.RemarksFrom = Remotecommunication.RemarksFrom;
                    communication.Remarks = Remotecommunication.Remarks;
                    communication.RemarksTo = Remotecommunication.RemarksTo;
                    communication.SendEmail = Remotecommunication.SendEmail;
                    communication.SetReminder = Remotecommunication.SetReminder;
                    communication.ReminderDate = Remotecommunication.ReminderDate;
                    communication.RemarksDate = Convert.ToDateTime(Remotecommunication.RemarksDate);
                }
                return communication;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<RfqItemModel> GetItemsByItemId(int id)
        {
            RfqItemModel model = new RfqItemModel();
            try
            {
                var items = from x in obj.RFQItems where x.RFQItemsId == id && x.DeleteFlag == false select x;
                foreach (var item in items)
                {
                    model.HSNCode = item.HSNCode;
                    model.QuotationQty = item.QuotationQty;
                    model.VendorModelNo = item.VendorModelNo;
                    model.RequsetRemarks = item.RequestRemarks;
                    model.CGSTPercentage = item.CGSTPercentage;
                    model.IGSTPercentage = item.IGSTPercentage;
                    model.SGSTPercentage = item.SGSTPercentage;
                    model.CustomDuty = item.CustomDuty;
                    model.FreightAmount = item.FreightAmount;
                    model.FreightPercentage = item.FreightPercentage;
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<RfqItemModel> GetItemByItemId(int id)
        {
            RfqItemModel model = new RfqItemModel();
            try
            {
                //var items = from x in obj.RFQItems where x.RFQItemsId == id && x.DeleteFlag == false select x;
                var items = obj.RFQItems_N.Where(x => x.RFQItemsId == id && x.DeleteFlag == false).Include(x => x.RFQItemsInfo_N).FirstOrDefault();
                model.HSNCode = items.HSNCode;
                model.QuotationQty = Convert.ToDouble(items.QuotationQty);
                model.VendorModelNo = items.VendorModelNo;
                model.RequsetRemarks = items.RequestRemarks;
                model.CGSTPercentage = Convert.ToDecimal(items.CGSTPercentage);
                model.IGSTPercentage = Convert.ToDecimal(items.IGSTPercentage);
                model.SGSTPercentage = Convert.ToDecimal(items.SGSTPercentage);
                model.CustomDuty = Convert.ToDecimal(items.CustomDuty);
                model.PFPercentage = Convert.ToDecimal(items.PFPercentage);
                model.PFAmount = Convert.ToDecimal(items.PFAmount);
                model.FreightAmount = Convert.ToDecimal(items.FreightAmount);
                model.FreightPercentage = Convert.ToDecimal(items.FreightPercentage);


                var price = obj.RFQItemsInfoes.Where(x => x.RFQItemsId == items.RFQItemsId).FirstOrDefault();
                model.ItemUnitPrice = (price.UnitPrice) * (Convert.ToDecimal(price.Qty));

                decimal Discountpercentage = price.DiscountPercentage;
                decimal Discount = price.Discount;
                if (Discountpercentage != 0)
                {
                    model.DiscountAmount = (model.ItemUnitPrice * Discountpercentage) / 100;
                }
                else
                {
                    model.DiscountAmount = Discount;
                }
                if (model.CustomDuty != 0)
                {
                    model.CustomDutyAmount = Convert.ToDecimal((model.ItemUnitPrice * items.CustomDuty) / 100);
                    if (items.taxInclusiveOfDiscount == false)
                    {
                        if (Discount != 0)
                        {
                            model.DiscountAmount = Discount;
                            model.NetAmount = model.ItemUnitPrice - Discount;
                            model.TotalAmount = model.NetAmount + model.CustomDutyAmount;
                            if (model.PFAmount != 0)
                            {
                                model.FinalNetAmount = model.TotalAmount + model.PFAmount;
                            }
                            else
                            {
                                model.FinalNetAmount = model.TotalAmount;
                            }
                        }
                        else
                        {
                            model.Discountpercentage = Discountpercentage;
                            model.DiscountAmount = (model.ItemUnitPrice) * (Discountpercentage / 100);
                            model.NetAmount = (model.ItemUnitPrice) - (model.DiscountAmount);
                            if (model.PFPercentage != 0)
                            {
                                model.PFAmount = (model.NetAmount) * (model.PFPercentage) / 100;
                                model.FinalNetAmount = model.NetAmount + model.CustomDuty + model.PFAmount;
                            }
                            else
                            {
                                model.FinalNetAmount = model.NetAmount + model.CustomDuty;
                            }
                        }

                    }
                    else
                    {
                        if (Discountpercentage != 0 || Discount != 0)
                        {
                            if (Discount != 0)
                            {
                                model.DiscountAmount = Discount;
                                model.NetAmount = (model.ItemUnitPrice) - (model.DiscountAmount);
                                model.TotalAmount = model.NetAmount + model.CustomDutyAmount;
                                if (model.PFAmount != 0)
                                {
                                    model.FinalNetAmount = model.TotalAmount + model.CustomDutyAmount + model.PFAmount;
                                }
                                else
                                {
                                    model.FinalNetAmount = model.TotalAmount + model.CustomDutyAmount;
                                }
                            }
                            else
                            {
                                model.DiscountAmount = (model.ItemUnitPrice) * (Discountpercentage / 100);
                                model.NetAmount = (model.ItemUnitPrice) - (model.DiscountAmount);
                                if (model.PFPercentage != 0)
                                {
                                    model.PFAmount = (model.ItemUnitPrice) * (model.PFPercentage) / 100;
                                    model.TotalAmount = model.NetAmount + model.CustomDuty + model.PFAmount;
                                }
                                else
                                {
                                    model.FinalNetAmount = model.NetAmount + model.CustomDuty;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (items.taxInclusiveOfDiscount == false || items.taxInclusiveOfDiscount == null)
                    {
                        if (model.DiscountAmount != 0 || model.PFAmount != 0 || model.FreightAmount != 0)
                        {
                            //model.DiscountAmount = Discount;
                            model.NetAmount = (model.ItemUnitPrice) - (model.DiscountAmount);
                            if (model.FreightAmount != 0 || model.FreightPercentage != 0)
                            {
                                model.TotalAmount = (model.FreightPercentage * model.NetAmount) / 100 + model.FreightAmount + model.NetAmount;
                                model.SGSTAmount = (model.TotalAmount * model.SGSTPercentage) / 100;
                                model.CGSTAmount = (model.TotalAmount * model.CGSTPercentage) / 100;
                                model.IGSTAmount = (model.TotalAmount * model.IGSTPercentage) / 100;
                                model.TotalTaxAmount = model.SGSTAmount + model.CGSTAmount + model.IGSTAmount;
                                if (model.PFAmount != 0 || model.PFPercentage != 0)
                                {
                                    model.PFAmount = (model.PFPercentage * model.TotalAmount) / 100 + model.PFAmount + model.TotalAmount;
                                    model.FinalNetAmount = model.TotalAmount + model.TotalTaxAmount + model.PFAmount + (model.PFPercentage * model.TotalAmount) / 100;
                                }
                                else
                                {
                                    model.FinalNetAmount = model.TotalAmount + model.TotalTaxAmount;
                                }
                            }
                            else
                            {
                                model.SGSTAmount = (model.NetAmount * model.SGSTPercentage) / 100;
                                model.CGSTAmount = (model.NetAmount * model.CGSTPercentage) / 100;
                                model.IGSTAmount = (model.NetAmount * model.IGSTPercentage) / 100;
                                model.TotalTaxAmount = model.SGSTAmount + model.CGSTAmount + model.IGSTAmount;
                                if (model.PFAmount != 0)
                                {
                                    model.FinalNetAmount = model.NetAmount + model.TotalTaxAmount + model.PFAmount;
                                }
                                else
                                {
                                    model.FinalNetAmount = model.NetAmount + model.TotalTaxAmount;
                                }
                            }
                        }
                        //else
                        //{
                        //    model.DiscountAmount = (model.ItemUnitPrice * Discountpercentage) / 100;
                        //    model.NetAmount = (model.ItemUnitPrice) - (model.DiscountAmount);
                        //    model.SGSTAmount = (model.NetAmount * model.SGSTPercentage) / 100;
                        //    model.CGSTAmount = (model.NetAmount * model.CGSTPercentage) / 100;
                        //    model.IGSTAmount = (model.NetAmount * model.IGSTPercentage) / 100;
                        //    model.TotalTaxAmount = model.SGSTAmount + model.CGSTAmount + model.IGSTAmount;
                        //    if (model.PFPercentage != 0)
                        //    {
                        //        model.PFAmount = (model.NetAmount * model.PFPercentage) / 100;
                        //        model.FinalNetAmount = model.NetAmount + model.TotalTaxAmount + model.PFAmount;
                        //    }
                        //    else
                        //    {
                        //        model.FinalNetAmount = model.NetAmount + model.TotalTaxAmount;
                        //    }
                        //}
                    }
                    else
                    {
                        if (model.DiscountAmount != 0 || model.PFAmount != 0 || model.FreightAmount != 0)
                        {
                            //model.NetAmount = model.ItemUnitPrice - model.DiscountAmount;
                            if (model.FreightAmount != 0 || model.FreightPercentage != 0)
                            {
                                model.TotalAmount = (model.FreightPercentage * model.ItemUnitPrice) / 100 + model.FreightAmount + model.ItemUnitPrice;
                                model.NetAmount = (model.ItemUnitPrice) - (model.DiscountAmount);
                                model.SGSTAmount = (model.TotalAmount * model.SGSTPercentage) / 100;
                                model.CGSTAmount = (model.TotalAmount * model.CGSTPercentage) / 100;
                                model.IGSTAmount = (model.TotalAmount * model.IGSTPercentage) / 100;
                                model.TotalTaxAmount = model.SGSTAmount + model.CGSTAmount + model.IGSTAmount;
                                if (model.PFAmount != 0 || model.PFPercentage != 0)
                                {
                                    model.PFAmount = (model.PFPercentage * model.TotalAmount) / 100 + model.PFAmount;
                                    model.FinalNetAmount = model.TotalAmount + model.TotalTaxAmount + model.PFAmount - model.DiscountAmount;
                                }
                                else
                                {
                                    model.FinalNetAmount = model.TotalAmount + model.TotalTaxAmount - model.DiscountAmount;
                                }
                            }
                            else
                            {
                                model.SGSTAmount = (model.ItemUnitPrice * model.SGSTPercentage) / 100;
                                model.CGSTAmount = (model.ItemUnitPrice * model.CGSTPercentage) / 100;
                                model.IGSTAmount = (model.ItemUnitPrice * model.IGSTPercentage) / 100;
                                model.TotalTaxAmount = model.SGSTAmount + model.CGSTAmount + model.IGSTAmount;
                                if (model.PFAmount != 0)
                                {
                                    model.FinalNetAmount = model.ItemUnitPrice + model.TotalTaxAmount + model.PFAmount - model.DiscountAmount;
                                }
                                else
                                {
                                    model.FinalNetAmount = model.NetAmount + model.TotalTaxAmount - model.DiscountAmount;
                                }
                            }
                            //else
                            //{
                            //    model.DiscountAmount = (model.ItemUnitPrice * Discountpercentage) / 100;
                            //    model.SGSTAmount = (model.ItemUnitPrice * model.SGSTPercentage) / 100;
                            //    model.CGSTAmount = (model.ItemUnitPrice * model.CGSTPercentage) / 100;
                            //    model.IGSTAmount = (model.ItemUnitPrice * model.IGSTPercentage) / 100;
                            //    model.TotalTaxAmount = model.SGSTAmount + model.CGSTAmount + model.IGSTAmount;
                            //    model.NetAmount = model.ItemUnitPrice - model.DiscountAmount;
                            //    if (model.PFPercentage != 0)
                            //    {
                            //        model.PFAmount = (model.ItemUnitPrice * model.PFPercentage) / 100;
                            //        model.FinalNetAmount = model.NetAmount + model.TotalTaxAmount + model.PFAmount;
                            //    }
                            //    else
                            //    {
                            //        model.FinalNetAmount = model.NetAmount + model.TotalTaxAmount;
                            //    }
                            //}
                        }
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<VendormasterModel> GetAllvendorList()
        {
            List<VendormasterModel> vendor = new List<VendormasterModel>();
            try
            {
                var data = obj.VendorMasters.Where(x => x.Deleteflag == false).ToList();
                vendor = data.Select(x => new VendormasterModel()
                {
                    ContactNo = x.ContactNo,
                    Vendorid = x.Vendorid,
                    VendorCode = x.VendorCode,
                    VendorName = x.VendorName,

                }).ToList();
                return vendor;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<VendormasterModel> GetvendorById(int id)
        {
            VendormasterModel vendor = new VendormasterModel();
            try
            {
                var data = obj.VendorMasters.Where(x => x.Vendorid == id && x.Deleteflag == false).SingleOrDefault();
                if (data != null)
                {
                    vendor.ContactNo = data.ContactNo;
                    vendor.Emailid = data.Emailid;
                    vendor.VendorCode = data.VendorCode;
                    vendor.VendorName = data.VendorName;
                }
                return vendor;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<statuscheckmodel> InsertVendorterms(VendorRfqtermModel vendor)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                if (vendor != null)
                {
                    var data = new VendorRFQTerm();
                    data.VendorTermsid = vendor.VendorTermsid;
                    data.VendorID = vendor.VendorID;
                    data.Terms = vendor.Terms;
                    data.Indexno = vendor.Indexno;
                    obj.VendorRFQTerms.Add(data);
                    obj.SaveChanges();
                }
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> UpdateRfqStatus(int id)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = obj.RFQStatus.Select(x => x.RfqStatusId).ToList();

                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<RfqRevisionModel>> GetAllRFQs()
        {
            throw new NotImplementedException();
        }
        public async Task<statuscheckmodel> InsertRfqItemInfo(RfqItemModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                //for remoteserver
                if (model != null)
                {
                    vscm.Database.Connection.Open();
                    var Remotedata = new RemoteRFQItems_N();
                    var rfqRemoteitem = vscm.RemoteRFQItems_N.Where(x => x.RFQItemsId == model.RFQItemsId).FirstOrDefault();
                    rfqRemoteitem.HSNCode = model.HSNCode;
                    rfqRemoteitem.QuotationQty = model.QuotationQty;
                    rfqRemoteitem.VendorModelNo = model.VendorModelNo;
                    rfqRemoteitem.IGSTPercentage = model.IGSTPercentage;
                    rfqRemoteitem.SGSTPercentage = model.SGSTPercentage;
                    rfqRemoteitem.CGSTPercentage = model.CGSTPercentage;
                    rfqRemoteitem.MfgModelNo = model.MfgModelNo;
                    rfqRemoteitem.MfgPartNo = model.MfgPartNo;
                    rfqRemoteitem.PFAmount = model.PFAmount;
                    rfqRemoteitem.PFPercentage = model.PFPercentage;
                    rfqRemoteitem.FreightAmount = model.FreightAmount;
                    rfqRemoteitem.CustomDuty = model.CustomDuty;
                    rfqRemoteitem.taxInclusiveOfDiscount = model.taxInclusiveOfDiscount;

                    vscm.RemoteRFQItems_N.Add(Remotedata);
                    vscm.SaveChanges();
                    foreach (var item in model.iteminfo)
                    {
                        var remoteinfo = new RemoteRFQItemsInfo_N()
                        {
                            RFQItemsId = item.RFQItemsId,
                            Qty = item.Qunatity,
                            UOM = item.UOM,
                            UnitPrice = item.UnitPrice,
                            DiscountPercentage = item.DiscountPercentage,
                            Discount = item.Discount,
                            CurrencyId = item.CurrencyID,
                            CurrencyValue = item.CurrencyValue,
                            Remarks = item.Remarks,
                            DeliveryDate = item.DeliveryDate,
                            //GSTPercentage = item.GSTPercentage,
                            SyncDate = System.DateTime.Now
                        };
                        vscm.RemoteRFQItemsInfo_N.Add(remoteinfo);
                        vscm.SaveChanges();
                    }
                }
                //vscm.Database.Connection.Close();
                ///for local datbase
                var data = new RFQItem();
                //obj.Database.Connection.Open();
                var rfqitem = obj.RFQItems.Where(x => x.RFQItemsId == model.RFQItemsId).FirstOrDefault();
                rfqitem.HSNCode = model.HSNCode;
                rfqitem.QuotationQty = model.QuotationQty;
                rfqitem.VendorModelNo = model.VendorModelNo;
                rfqitem.MfgModelNo = model.MfgModelNo;
                rfqitem.MfgPartNo = model.MfgPartNo;
                obj.RFQItems.Add(data);
                obj.SaveChanges();
                foreach (var item in model.iteminfo)
                {
                    var info = new RFQItemsInfo()
                    {
                        RFQItemsId = item.RFQItemsId,
                        Qty = item.Qunatity,
                        UOM = item.UOM,
                        UnitPrice = item.UnitPrice,
                        DiscountPercentage = item.DiscountPercentage,
                        Discount = item.Discount,
                        CurrencyId = item.CurrencyID,
                        CurrencyValue = item.CurrencyValue,
                        Remarks = item.Remarks,
                        DeliveryDate = item.DeliveryDate
                    };
                    obj.RFQItemsInfoes.Add(info);
                    obj.SaveChanges();
                }
                obj.Database.Connection.Close();

                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> DeleteRfqIteminfoByid(List<int> id)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                vscm.Database.Connection.Open();
                var Remotedata = vscm.RemoteRFQItemsInfo_N.Where(x => id.Contains(x.RFQSplitItemId) && x.DeleteFlag == false).FirstOrDefault();
                if (Remotedata != null)
                {
                    Remotedata.DeleteFlag = true;
                    vscm.SaveChanges();
                }
                vscm.Database.Connection.Close();

                obj.Database.Connection.Open();
                var Localdata = obj.RFQItemsInfoes.Where(x => id.Contains(x.RFQSplitItemId) && x.DeleteFlag == false).FirstOrDefault();
                if (Localdata != null)
                {
                    Localdata.DeleteFlag = true;
                    obj.SaveChanges();
                }
                status.Sid = Localdata.RFQItemsId;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> DeleteRfqitemandinfosById(int id)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                //remotedata
                vscm.Database.Connection.Open();
                var Remotedata = vscm.RemoteRFQItems_N.Where(x => x.RFQItemsId == id && x.DeleteFlag == false).FirstOrDefault();
                if (Remotedata != null)
                {
                    Remotedata.DeleteFlag = true;
                    vscm.SaveChanges();

                    var remoteitemsdata = vscm.RemoteRFQItemsInfo_N.Where(x => x.RFQItemsId == Remotedata.RFQItemsId && x.DeleteFlag == false).ToList();
                    if (remoteitemsdata != null)
                    {
                        foreach (var item in remoteitemsdata)
                        {
                            item.DeleteFlag = true;
                            vscm.SaveChanges();
                        }
                    }
                    //int Itemid = Remotedata.RFQItemsId;
                }
                vscm.Database.Connection.Close();

                //localdata
                obj.Database.Connection.Open();
                var Localdata = obj.RFQItems.Where(x => x.RFQItemsId == id && x.DeleteFlag == false).FirstOrDefault();
                if (Localdata != null)
                {
                    Localdata.DeleteFlag = true;
                    obj.SaveChanges();

                    var localitemsdata = obj.RFQItemsInfoes.Where(x => x.RFQItemsId == Remotedata.RFQItemsId && x.DeleteFlag == false).ToList();
                    if (localitemsdata != null)
                    {
                        foreach (var item in localitemsdata)
                        {
                            item.DeleteFlag = true;
                            obj.SaveChanges();
                        }
                    }
                }
                int Itemid = Localdata.RFQItemsId;
                obj.Database.Connection.Close();
                status.Sid = Itemid;
                return status;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<statuscheckmodel> UpdateRfqItemInfoById(RfqItemInfoModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                //remote data
                var remoteitem = vscm.RemoteRFQItemsInfo_N.Where(x => x.RFQItemsId == model.RFQItemsId).FirstOrDefault();
                remoteitem.Discount = model.Discount;
                remoteitem.DiscountPercentage = model.DiscountPercentage;
                remoteitem.Qty = model.Qunatity;
                remoteitem.UnitPrice = model.UnitPrice;
                remoteitem.UOM = model.UOM;
                remoteitem.CurrencyValue = model.CurrencyValue;
                remoteitem.CurrencyId = model.CurrencyID;
                remoteitem.Remarks = model.Remarks;
                remoteitem.DeliveryDate = model.DeliveryDate;

                vscm.RemoteRFQItemsInfo_N.Add(remoteitem);
                vscm.SaveChanges();
                int remoteitemid = remoteitem.RFQItemsId;
                vscm.Database.Connection.Close();

                obj.Database.Connection.Open();
                var localitem = obj.RFQItemsInfoes.Where(x => x.RFQItemsId == model.RFQItemsId).FirstOrDefault();
                localitem.Discount = model.Discount;
                localitem.DiscountPercentage = model.DiscountPercentage;
                localitem.Qty = model.Qunatity;
                localitem.UnitPrice = model.UnitPrice;
                localitem.UOM = model.UOM;
                localitem.CurrencyValue = model.CurrencyValue;
                localitem.CurrencyId = model.CurrencyID;
                localitem.Remarks = model.Remarks;
                localitem.DeliveryDate = model.DeliveryDate;
                obj.RFQItemsInfoes.Add(localitem);
                obj.SaveChanges();
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<RfqItemModel> GetRfqItemByMPrId(int id)
        {
            RfqItemModel itemmodel = new RfqItemModel();
            try
            {
                vscm.Database.Connection.Open();
                RfqItemInfoModel iteminfo = new RfqItemInfoModel();
                var itemdetails = vscm.RemoteRFQItems_N.Where(x => x.MPRItemDetailsid == id).Include(x => x.RemoteRFQItemsInfo_N).FirstOrDefault();
                itemmodel.HSNCode = itemdetails.HSNCode;
                itemmodel.RFQItemID = itemdetails.RFQItemsId;
                itemmodel.QuotationQty = Convert.ToDouble(itemdetails.QuotationQty);
                itemmodel.VendorModelNo = itemdetails.VendorModelNo;
                itemmodel.RequsetRemarks = itemdetails.RequestRemarks;
                foreach (var item in itemdetails.RemoteRFQItemsInfo_N)
                {
                    iteminfo.RFQItemsId = item.RFQItemsId;
                    iteminfo.RFQSplitItemId = item.RFQSplitItemId;
                    iteminfo.UnitPrice = item.UnitPrice;
                    iteminfo.UOM = item.UOM;
                    iteminfo.Remarks = item.Remarks;
                    iteminfo.DiscountPercentage = item.DiscountPercentage;
                    iteminfo.Discount = item.Discount;
                    iteminfo.DeliveryDate = item.DeliveryDate;
                    iteminfo.CurrencyID = item.CurrencyId;
                    iteminfo.CurrencyValue = item.CurrencyValue;
                    itemmodel.iteminfo.Add(iteminfo);
                }
                return itemmodel;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<RfqRevisionModel> GetRfqDetailsById(int revisionId)
        {
            obj.Configuration.ProxyCreationEnabled = false;
            RfqRevisionModel revision = new RfqRevisionModel();
            try
            {
                var localrevision = obj.RFQRevisions_N.Where(x => x.rfqRevisionId == revisionId && x.DeleteFlag == false).Include(x => x.RFQMaster).Include(x => x.RFQItems_N).FirstOrDefault();
                if (localrevision != null)
                {
                    revision.RfqMasterId = localrevision.rfqMasterId;
                    revision.RfqRevisionNo = localrevision.RevisionNo;
                    revision.CreatedBy = localrevision.CreatedBy;
                    revision.CreatedDate = localrevision.CreatedDate;
                    revision.RfqValidDate = localrevision.RFQValidDate;
                    revision.PackingForwading = localrevision.PackingForwarding;
                    revision.salesTax = localrevision.SalesTax;
                    revision.Insurance = localrevision.Insurance;
                    revision.CustomsDuty = localrevision.CustomsDuty;
                    revision.PaymentTermDays = localrevision.PaymentTermDays;
                    revision.PaymentTermRemarks = localrevision.PaymentTermRemarks;
                    revision.BankGuarantee = localrevision.BankGuarantee;
                    revision.DeliveryMaxWeeks = localrevision.DeliveryMaxWeeks;
                    revision.DeliveryMinWeeks = localrevision.DeliveryMinWeeks;


                    var rfqmasters = from x in obj.RFQMasters where x.RfqMasterId == localrevision.rfqMasterId select x;
                    var masters = new RFQMasterModel();
                    foreach (var item in rfqmasters)
                    {
                        masters.RfqMasterId = item.RfqMasterId;
                        masters.RfqNo = item.RFQNo;
                        masters.RfqUniqueNo = item.RFQUniqueNo;
                        masters.VendorId = item.VendorId;
                        var vendorMaster = obj.VendorMasters.FirstOrDefault(li => li.Vendorid == item.VendorId);
                        masters.Vendor = new VendormasterModel();
                        masters.Vendor.VendorName = vendorMaster.VendorName;
                        masters.Vendor.Emailid = vendorMaster.Emailid;
                        masters.MPRRevisionId = (int)item.MPRRevisionId;
                        masters.CreatedBy = item.CreatedBy;
                    }
                    revision.mprIncharges = obj.MPRIncharges.Where(li => li.RevisionId == masters.MPRRevisionId).ToList();
                    revision.rfqmaster = masters;
                    var rfqitemss = obj.RFQItems_N.Where(x => x.RFQRevisionId == localrevision.rfqRevisionId).ToList();
                    foreach (var item in rfqitemss)
                    {
                        //revision.rfqitem= localrevision.RFQItems.Select(x => new RfqItemModel()
                        //  {
                        //      HSNCode = item.HSNCode,
                        //      RFQItemID = item.RFQItemID,
                        //      RFQRevisionId = item.RFQRevisionId,
                        //      QuotationQty = item.QuotationQty,
                        //      VendorModelNo = item.VendorModelNo,
                        //      RequsetRemarks = item.RequsetRemarks
                        //  }).ToList();
                        MPRItemInfo mprItem = obj.MPRItemInfoes.FirstOrDefault(li => li.Itemdetailsid == item.MPRItemDetailsid);
                        RfqItemModel rfqitems = new RfqItemModel();
                        rfqitems.HSNCode = item.HSNCode;
                        var rfqIInfo= obj.RFQItemsInfo_N.Where(li => li.RFQItemsId == item.RFQItemsId).FirstOrDefault();
                        if(rfqIInfo!=null)
                        rfqitems.ItemUnitPrice = rfqIInfo.UnitPrice;
                        rfqitems.MRPItemsDetailsID = item.MPRItemDetailsid;
                        rfqitems.QuotationQty = Convert.ToDouble(item.QuotationQty);
                        rfqitems.RFQRevisionId = item.RFQRevisionId;
                        rfqitems.RFQItemID = item.RFQItemsId;
                        rfqitems.ItemName = obj.MaterialMasterYGS.FirstOrDefault(li => li.Material == mprItem.Itemid).Materialdescription;
                        rfqitems.ItemDescription = mprItem.ItemDescription;
                        var scmRfqdocs = obj.RFQDocuments.Where(li => li.rfqItemsid == item.RFQItemsId).ToList();
                        foreach (var items in scmRfqdocs)
                        {
                            RfqDocumentsModel rfqDocs = new RfqDocumentsModel();
                            rfqDocs.RfqDocumentId = items.RfqDocId;
                            rfqDocs.RfqRevisionId = items.rfqRevisionId;
                            rfqDocs.RfqItemsId = items.rfqItemsid;
                            rfqDocs.DocumentName = items.DocumentName;
                            rfqDocs.Path = items.Path;
                            rfqDocs.UploadedBy = items.UploadedBy;
                            rfqDocs.UploadedDate = items.UploadedDate;
                            rfqDocs.StatusRemarks = items.StatusRemarks;
                            rfqDocs.Status = items.Status;
                            rfqDocs.StatusBy = items.StatusBy;
                            rfqDocs.Statusdate = items.StatusDate;
                            rfqitems.RFQDocuments.Add(rfqDocs);
                        }
                        revision.rfqitem.Add(rfqitems);
                    }
                    var rfqterms = obj.RFQTerms.Where(x => x.RFQrevisionId == revisionId).ToList();
                    foreach (var item in rfqterms)
                    {
                        RFQTermsModel terms = new RFQTermsModel();
                        terms.RfqTermsid = item.RfqTermsid;
                        terms.termsid = item.termsid;
                        terms.Terms = obj.YILTermsandConditions.FirstOrDefault(li => li.TermId == item.termsid).Terms;
                        terms.VendorResponse = item.VendorResponse;
                        terms.Remarks = item.Remarks;

                        revision.RFQTerms.Add(terms);
                    }
                    revision.rfqCommunications = obj.RFQCommunications.Where(li => li.RfqRevisionId == revisionId).ToList();
                    foreach (RFQCommunication item in revision.rfqCommunications)
                    {
                        item.Employee = obj.VendorEmployeeViews.Where(li => li.EmployeeNo == item.RemarksFrom).FirstOrDefault();

                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return revision;
        }
        public bool updateRfqDocStatus(List<RFQDocument> rfqDocs)
        {
            using (var Context = new YSCMEntities()) //ok
            {
                foreach (RFQDocument rfqdoc in rfqDocs)
                {
                    List<RFQDocument> rfqDocs1 = Context.RFQDocuments.Where(li => li.rfqItemsid == rfqdoc.rfqItemsid).ToList();

                    foreach (RFQDocument item in rfqDocs1)
                    {
                        item.Status = rfqdoc.Status;
                        item.StatusRemarks = rfqdoc.StatusRemarks;
                        item.StatusBy = rfqdoc.StatusBy;
                        item.StatusDate = rfqdoc.StatusDate;
                        //string query = "update RFQDocuments set Status = '" + rfqdoc.Status + "', StatusRemarks = '" + rfqdoc.StatusRemarks + "',StatusBy = '" + rfqdoc.StatusBy + "',StatusDate = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' where rfqItemsid = " + rfqdoc.rfqItemsid + " ";
                        //Context.Database.SqlQuery<bool>(query);
                        Context.SaveChanges();
                    }
                }
            }
            return true;
        }
        public async Task<statuscheckmodel> InsertSingleIteminfos(RfqItemInfoModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                vscm.Database.Connection.Open();
                var remoteiteminfo = new RemoteRFQItemsInfo_N();
                remoteiteminfo.UOM = model.UOM;
                remoteiteminfo.UnitPrice = model.UnitPrice;
                remoteiteminfo.RFQItemsId = model.RFQItemsId;
                remoteiteminfo.DiscountPercentage = model.DiscountPercentage;
                remoteiteminfo.Qty = model.Qunatity;
                remoteiteminfo.DeliveryDate = model.DeliveryDate;
                remoteiteminfo.CurrencyValue = model.CurrencyValue;
                remoteiteminfo.CurrencyId = model.CurrencyID;
                remoteiteminfo.Remarks = model.Remarks;

                vscm.RemoteRFQItemsInfo_N.Add(remoteiteminfo);
                vscm.SaveChanges();
                int Remotesplitid = remoteiteminfo.RFQSplitItemId;
                vscm.Database.Connection.Close();

                obj.Database.Connection.Open();
                var localiteminfo = new RFQItemsInfo();
                localiteminfo.RFQSplitItemId = Remotesplitid;
                localiteminfo.UOM = model.UOM;
                localiteminfo.UnitPrice = model.UnitPrice;
                localiteminfo.RFQItemsId = model.RFQItemsId;
                localiteminfo.Remarks = model.Remarks;
                localiteminfo.DiscountPercentage = model.DiscountPercentage;
                localiteminfo.Qty = model.Qunatity;
                localiteminfo.DeliveryDate = model.DeliveryDate;
                localiteminfo.CurrencyValue = model.CurrencyValue;
                localiteminfo.CurrencyId = model.CurrencyID;
                obj.RFQItemsInfoes.Add(localiteminfo);
                obj.SaveChanges();
                int localsplitid = localiteminfo.RFQSplitItemId;
                status.Sid = localsplitid;
                return status;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<statuscheckmodel> InsertBulkItemInfos(List<RfqItemInfoModel> model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                if (model != null)
                {
                    vscm.Database.Connection.Open();
                    var remoteiteminfo = new RemoteRFQItemsInfo_N();
                    foreach (var item in model)
                    {
                        remoteiteminfo.UOM = item.UOM;
                        remoteiteminfo.UnitPrice = item.UnitPrice;
                        remoteiteminfo.RFQItemsId = item.RFQItemsId;
                        //remoteiteminfo.GSTPercentage = item.GSTPercentage;
                        remoteiteminfo.DiscountPercentage = item.DiscountPercentage;
                        remoteiteminfo.Qty = item.Qunatity;
                        remoteiteminfo.DeliveryDate = item.DeliveryDate;
                        remoteiteminfo.CurrencyValue = item.CurrencyValue;
                        remoteiteminfo.CurrencyId = item.CurrencyID;
                        vscm.RemoteRFQItemsInfo_N.Add(remoteiteminfo);
                        vscm.SaveChanges();
                    }
                    //List<int> id=vscm.RemoteRFQItemsInfoes.Where
                    // vscm.SaveChanges();
                }
                vscm.Database.Connection.Close();
                if (model != null)
                {
                    obj.Database.Connection.Open();
                    var localiteminfo = new RFQItemsInfo();
                    foreach (var item in model)
                    {
                        localiteminfo.UOM = item.UOM;
                        localiteminfo.UnitPrice = item.UnitPrice;
                        localiteminfo.RFQItemsId = item.RFQItemsId;
                        //localiteminfo.GSTPercentage = item.GSTPercentage;
                        localiteminfo.DiscountPercentage = item.DiscountPercentage;
                        localiteminfo.Qty = item.Qunatity;
                        localiteminfo.DeliveryDate = item.DeliveryDate;
                        localiteminfo.CurrencyValue = item.CurrencyValue;
                        localiteminfo.CurrencyId = item.CurrencyID;
                        obj.RFQItemsInfoes.Add(localiteminfo);
                        obj.SaveChanges();
                    }
                }
                return status;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<statuscheckmodel> InsertRfqRemainder(RfqRemainderTrackingModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                vscm.Database.Connection.Open();
                var remotecommunications = vscm.RemoteRFQCommunications.Where(x => x.RfqCCid == model.rfqccid && x.DeleteFlag == false).FirstOrDefault();
                var remotetracking = new RemoteRFQReminderTracking();
                if (remotecommunications != null)
                {
                    remotetracking.rfqccid = model.rfqccid;
                    remotetracking.ReminderTo = model.ReminderTo;
                    remotetracking.MailsSentOn = model.MailsSentOn;
                    remotetracking.Acknowledgementon = model.Acknowledgementon;
                    remotetracking.AcknowledgementRemarks = model.AcknowledgementRemarks;
                    vscm.RemoteRFQReminderTrackings.Add(remotetracking);
                    vscm.SaveChanges();
                }
                int rid = remotetracking.Reminderid;
                vscm.Database.Connection.Close();

                var localtracking = new RFQReminderTracking();
                obj.Database.Connection.Open();
                var localcommunication = obj.RFQCommunications.Where(x => x.RfqCCid == model.rfqccid && x.DeleteFlag == false).FirstOrDefault();
                if (localcommunication != null)
                {
                    localtracking.Reminderid = rid;
                    localtracking.rfqccid = model.rfqccid;
                    localtracking.ReminderTo = model.ReminderTo;
                    localtracking.MailsSentOn = model.MailsSentOn;
                    localtracking.Acknowledgementon = model.Acknowledgementon;
                    localtracking.AcknowledgementRemarks = model.AcknowledgementRemarks;
                    obj.RFQReminderTrackings.Add(localtracking);
                    obj.SaveChanges();
                }
                status.Sid = rid;
                return status;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<List<UnitMasterModel>> GetUnitMasterList()
        {
            List<UnitMasterModel> model = new List<UnitMasterModel>();
            try
            {
                var unitmaster = obj.UnitMasters.Where(x => x.DeleteFlag == false).ToList();
                model = unitmaster.Select(x => new UnitMasterModel()
                {
                    UnitID = x.UnitId,
                    UnitName = x.UnitName,
                    //Isdeleted=x.DeleteFlag
                }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<RfqRemainderTrackingModel> getrfqremaindersById(int id)
        {
            RfqRemainderTrackingModel model = new RfqRemainderTrackingModel();
            try
            {
                var Tracking = obj.RFQReminderTrackings.Where(x => x.rfqccid == id && x.DeleteFlag == false).FirstOrDefault();
                if (Tracking != null)
                {
                    model.Reminderid = Tracking.Reminderid;
                    model.ReminderTo = Tracking.ReminderTo;
                    model.MailsSentOn = Tracking.MailsSentOn;
                    model.Acknowledgementon = Tracking.Acknowledgementon;
                    model.AcknowledgementRemarks = Tracking.AcknowledgementRemarks;
                }
                return model;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        //public statuscheckmodel InsertDocument(RfqDocumentsModel model)
        //{
        //    throw new NotImplementedException();
        //}
        public async Task<statuscheckmodel> Insertrfqvendorterms(RfqVendorTermModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            var rfqterms = new RemoteRFQVendorTerm();
            try
            {
                vscm.Database.Connection.Open();
                if (model != null)
                {
                    rfqterms.RFQversionId = model.RFQversionId;
                    rfqterms.VendorTermsid = model.VendorTermsid;
                    rfqterms.updatedBY = model.updatedBY;
                    rfqterms.UpdateDate = model.UpdateDate;
                }
                vscm.RemoteRFQVendorTerms.Add(rfqterms);
                vscm.SaveChanges();
                int remoterfqid = rfqterms.RFQTermsid;
                vscm.Database.Connection.Close();
                var rfqlocalterms = new RFQVendorTerm();
                obj.Database.Connection.Open();
                if (model != null)
                {
                    rfqlocalterms.RFQTermsid = remoterfqid;
                    rfqlocalterms.RFQversionId = model.RFQversionId;
                    rfqlocalterms.VendorTermsid = model.VendorTermsid;
                    rfqlocalterms.updatedBY = model.updatedBY;
                    rfqlocalterms.UpdateDate = model.UpdateDate;
                }
                obj.RFQVendorTerms.Add(rfqlocalterms);
                obj.SaveChanges();
                status.Sid = remoterfqid;
                return status;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<RfqVendorTermModel> getRfqVendorById(int id)
        {
            RfqVendorTermModel model = new RfqVendorTermModel();
            try
            {
                var remotedata = vscm.RemoteRFQVendorTerms.Where(x => x.RFQversionId == id).SingleOrDefault();
                model.RFQTermsid = remotedata.RFQTermsid;
                model.RFQversionId = remotedata.RFQversionId;
                var terms = from x in vscm.RemoteVendorRFQTerms where x.VendorTermsid == remotedata.VendorTermsid select x;
                var vendorterms = new VendorRfqtermModel();
                foreach (var item in terms)
                {
                    vendorterms.Terms = item.Terms;
                    vendorterms.Indexno = item.Indexno;
                    vendorterms.TermsCategoryId = item.TermsCategoryId;
                    vendorterms.VendorID = item.VendorID;
                }
                model.VendorRFQTerm = vendorterms;
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> RemoveRfqVendorTermsById(int id)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                vscm.Database.Connection.Open();
                var rfqterms = vscm.RemoteRFQVendorTerms.Where(x => x.RFQversionId == id && x.DeleteFlag == false).SingleOrDefault();
                if (rfqterms != null)
                {
                    rfqterms.DeleteFlag = true;
                    vscm.SaveChanges();
                }
                vscm.Database.Connection.Close();

                var rfqlocalterms = obj.RFQVendorTerms.Where(x => x.RFQversionId == id && x.DeleteFlag == false).SingleOrDefault();
                if (rfqlocalterms != null)
                {
                    rfqlocalterms.DeleteFlag = true;
                    obj.SaveChanges();
                }
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<VendorRfqtermModel> getVendorRfqTermsByid(int id)
        {
            VendorRfqtermModel rfqterm = new VendorRfqtermModel();
            try
            {
                var vendorrfq = vscm.RemoteVendorRFQTerms.Where(x => x.VendorTermsid == id && x.deleteFlag == false).SingleOrDefault();
                if (vendorrfq != null)
                {
                    rfqterm.VendorTermsid = vendorrfq.VendorTermsid;
                    rfqterm.TermsCategoryId = vendorrfq.TermsCategoryId;
                    rfqterm.VendorID = vendorrfq.VendorID;
                    rfqterm.Terms = vendorrfq.Terms;
                    rfqterm.Indexno = vendorrfq.Indexno;
                }
                return rfqterm;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> RemoveVendorRfqByid(int id)
        {
            statuscheckmodel status = new statuscheckmodel();
            vscm.Database.Connection.Open();
            try
            {
                var remotevendor = vscm.RemoteVendorRFQTerms.Where(x => x.VendorTermsid == id && x.deleteFlag == false).SingleOrDefault();
                if (remotevendor != null)
                {
                    remotevendor.deleteFlag = true;
                    vscm.SaveChanges();
                }
                var data = remotevendor.VendorTermsid;
                vscm.Database.Connection.Close();

                obj.Database.Connection.Open();
                var localvendor = obj.VendorRFQTerms.Where(x => x.VendorTermsid == data && x.deleteFlag == false).SingleOrDefault();
                if (localvendor != null)
                {
                    localvendor.deleteFlag = true;
                    obj.SaveChanges();
                }
                status.Sid = data;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> InsertNewCurrencyMaster(CurrencyMasterModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                obj.Database.Connection.Open();
                var data = new CurrencyMaster();
                if (model != null)
                {
                    data.CurrencyName = model.CurrencyName;
                    data.CurrencyCode = model.CurrencyCode;
                    data.UpdatedBy = model.UpdatedBy;
                    data.updateddate = model.updateddate;
                    data.DeletedBy = model.DeletedBy;
                    data.DeletedDate = model.DeletedDate;
                    data.DeleteFlag = false;
                }
                obj.CurrencyMasters.Add(data);
                obj.SaveChanges();
                Byte currenyid = data.CurrencyId;
                obj.Database.Connection.Close();
                var remotedata = new RemoteCurrencyMaster();
                vscm.Database.Connection.Open();
                if (model != null)
                {
                    remotedata.CurrencyId = currenyid;
                    remotedata.CurrencyCode = model.CurrencyCode;
                    remotedata.CurrencyName = model.CurrencyName;
                    remotedata.UpdatedBy = model.UpdatedBy;
                    remotedata.updateddate = model.updateddate;
                    remotedata.DeletedBy = model.DeletedBy;
                    remotedata.DeletedDate = model.DeletedDate;
                    remotedata.DeleteFlag = false;
                }
                vscm.RemoteCurrencyMasters.Add(remotedata);
                vscm.SaveChanges();
                status.Sid = currenyid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> UpdateNewCurrencyMaster(CurrencyMasterModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                obj.Database.Connection.Open();
                var data = obj.CurrencyMasters.Where(x => x.CurrencyId == model.CurrenyId && x.DeleteFlag == false).FirstOrDefault();
                if (model != null)
                {
                    data.CurrencyName = model.CurrencyName;
                    data.CurrencyCode = model.CurrencyCode;
                    data.UpdatedBy = model.UpdatedBy;
                    data.updateddate = model.updateddate;
                    data.DeletedBy = model.DeletedBy;
                    data.DeletedDate = model.DeletedDate;
                    data.DeleteFlag = false;
                }
                obj.CurrencyMasters.Add(data);
                obj.SaveChanges();
                Byte currenyid = data.CurrencyId;
                obj.Database.Connection.Close();
                var remotedata = new RemoteCurrencyMaster();
                vscm.Database.Connection.Open();
                if (model != null)
                {
                    remotedata.CurrencyId = currenyid;
                    remotedata.CurrencyCode = model.CurrencyCode;
                    remotedata.CurrencyName = model.CurrencyName;
                    remotedata.UpdatedBy = model.UpdatedBy;
                    remotedata.updateddate = model.updateddate;
                    remotedata.DeletedBy = model.DeletedBy;
                    remotedata.DeletedDate = model.DeletedDate;
                    remotedata.DeleteFlag = false;
                }
                vscm.RemoteCurrencyMasters.Add(remotedata);
                vscm.SaveChanges();
                status.Sid = currenyid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> InsertCurrentCurrencyHistory(CurrencyHistoryModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = new CurrencyHistory();
                obj.Database.Connection.Open();
                if (model != null)
                {
                    data.CurrencyId = model.CurrencyId;
                    data.CurrencyValue = model.CurrencyValue;
                    data.EffectiveFrom = model.EffectiveFrom;
                    data.EffectiveTo = model.EffectiveTo;
                    data.UpdatedBy = model.UpdatedBy;
                    data.UpdatedDate = model.UpdatedDate;
                    data.IsActive = true;
                }
                obj.CurrencyHistories.Add(data);
                obj.SaveChanges();
                int historyid = data.CurrencyHistoryId;
                obj.Database.Connection.Close();

                var Remotedata = new RemoteCurrencyHistory();
                vscm.Database.Connection.Open();
                if (model != null)
                {
                    Remotedata.CurrencyId = model.CurrencyId;
                    Remotedata.CurrencyValue = model.CurrencyValue;
                    Remotedata.EffectiveFrom = model.EffectiveFrom;
                    Remotedata.EffectiveTo = model.EffectiveTo;
                    Remotedata.UpdatedBy = model.UpdatedBy;
                    Remotedata.UpdatedDate = model.UpdatedDate;
                    Remotedata.IsActive = true;
                }
                vscm.RemoteCurrencyHistories.Add(Remotedata);
                vscm.SaveChanges();
                status.Sid = historyid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> UpdateCurrentCurrencyHistory(CurrencyHistoryModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {

                obj.Database.Connection.Open();
                var data = obj.CurrencyHistories.Where(x => x.CurrencyId == model.CurrencyId).FirstOrDefault();
                if (model != null)
                {
                    data.CurrencyId = model.CurrencyId;
                    data.CurrencyValue = model.CurrencyValue;
                    data.EffectiveFrom = model.EffectiveFrom;
                    data.EffectiveTo = model.EffectiveTo;
                    data.UpdatedBy = model.UpdatedBy;
                    data.UpdatedDate = model.UpdatedDate;
                    data.IsActive = true;
                }
                obj.CurrencyHistories.Add(data);
                obj.SaveChanges();
                int historyid = data.CurrencyHistoryId;
                obj.Database.Connection.Close();


                vscm.Database.Connection.Open();
                var Remotedata = vscm.RemoteCurrencyHistories.Where(x => x.CurrencyId == model.CurrencyId).FirstOrDefault();
                if (model != null)
                {
                    Remotedata.CurrencyId = model.CurrencyId;
                    Remotedata.CurrencyValue = model.CurrencyValue;
                    Remotedata.EffectiveFrom = model.EffectiveFrom;
                    Remotedata.EffectiveTo = model.EffectiveTo;
                    Remotedata.UpdatedBy = model.UpdatedBy;
                    Remotedata.UpdatedDate = model.UpdatedDate;
                    Remotedata.IsActive = true;
                }
                vscm.RemoteCurrencyHistories.Add(Remotedata);
                vscm.SaveChanges();
                status.Sid = historyid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CurrencyMasterModel>> GetAllMasterCurrency()
        {
            List<CurrencyMasterModel> model = new List<CurrencyMasterModel>();
            try
            {
                var currencydata = obj.CurrencyMasters.Where(x => x.DeleteFlag == false).ToList();
                model = currencydata.Select(x => new CurrencyMasterModel()
                {
                    CurrencyCode = x.CurrencyCode,
                    CurrencyName = x.CurrencyName,
                    CurrenyId = x.CurrencyId,
                    UpdatedBy = x.UpdatedBy,
                    updateddate = x.updateddate,
                    DeletedBy = x.DeletedBy,
                    DeletedDate = x.DeletedDate
                }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CurrencyMasterModel> GetMasterCurrencyById(int currencyId)
        {
            CurrencyMasterModel model = new CurrencyMasterModel();
            try
            {
                var currencydata = obj.CurrencyMasters.Where(x => x.CurrencyId == currencyId && x.DeleteFlag == false).FirstOrDefault<CurrencyMaster>();
                model.CurrencyName = currencydata.CurrencyName;
                model.CurrencyCode = currencydata.CurrencyCode;
                model.UpdatedBy = currencydata.UpdatedBy;
                model.updateddate = currencydata.updateddate;
                model.DeletedBy = currencydata.DeletedBy;
                model.DeletedDate = currencydata.DeletedDate;
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> RemoveMasterCurrencyById(int currencyId)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var currencydata = obj.CurrencyMasters.Where(x => x.CurrencyId == currencyId && x.DeleteFlag == false).FirstOrDefault<CurrencyMaster>();
                if (currencydata != null)
                {
                    currencydata.DeleteFlag = true;
                    obj.SaveChanges();

                    var currencyhistorydata = obj.CurrencyHistories.Where(x => x.CurrencyId == currencydata.CurrencyId).FirstOrDefault<CurrencyHistory>();
                    if (currencyhistorydata != null)
                    {
                        currencyhistorydata.IsActive = false;
                        obj.SaveChanges();
                    }
                }
                int id = currencydata.CurrencyId;
                status.Sid = id;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CurrencyHistoryModel> GetcurrencyHistoryById(int currencyId)
        {
            CurrencyHistoryModel model = new CurrencyHistoryModel();
            try
            {
                var currenyhistorydata = obj.CurrencyHistories.Where(x => x.CurrencyId == currencyId && x.IsActive == true).ToList();
                foreach (var item in currenyhistorydata)
                {
                    model.CurrencyHistoryId = item.CurrencyHistoryId;
                    model.CurrencyValue = item.CurrencyValue;
                    model.editedBy = item.editedBy;
                    model.EditedDate = item.EditedDate;
                    model.EffectiveFrom = item.EffectiveFrom;
                    model.EffectiveTo = item.EffectiveTo;
                    model.UpdatedBy = item.UpdatedBy;
                    model.UpdatedDate = item.UpdatedDate;
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<MPRApproversViewModel>> GetAllMPRApprovers()
        {
            List<MPRApproversViewModel> model = new List<MPRApproversViewModel>();
            try
            {
                var data = obj.MPRApproversViews.SqlQuery("select * from MPRApproversView");
                model = data.Select(x => new MPRApproversViewModel()
                {
                    EmployeeNo = x.EmployeeNo,
                    Name = x.Name,
                    //DeactivatedBy = x.DeactivatedBy,
                    //DeactivatedOn = x.DeactivatedOn
                }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> InsertMprBuyerGroups(MPRBuyerGroupModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = new MPRBuyerGroup();
                if (model != null && model.BuyerGroupId == 0)
                {
                    // data.BuyerGroupId = model.BuyerGroupId;
                    data.BuyerGroup = model.BuyerGroup;
                    data.BoolInUse = true;
                }
                obj.MPRBuyerGroups.Add(data);
                obj.SaveChanges();
                byte memberid = data.BuyerGroupId;
                foreach (var item in model.MPRBuyerGroup)
                {
                    var groupmembers = new MPRBuyerGroupMember()
                    {
                        BuyerGroupId = memberid,
                        GroupMember = item.GroupMember
                    };
                    data.MPRBuyerGroupMembers.Add(groupmembers);
                    obj.SaveChanges();
                }
                int id = data.BuyerGroupId;
                status.Sid = id;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> UpdateMprBuyerGroups(MPRBuyerGroupModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = obj.MPRBuyerGroups.Where(x => x.BuyerGroupId == model.BuyerGroupId).FirstOrDefault();
                if (model != null)
                {
                    data.BuyerGroup = model.BuyerGroup;
                    data.BoolInUse = model.BoolInUse;
                    obj.SaveChanges();
                }
                int id = data.BuyerGroupId;
                status.Sid = id;
                return status;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<statuscheckmodel> InsertMprBuyerGroupMembers(MPRBuyerGroupMemberModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                if (true)
                {

                }
                var groupmember = new MPRBuyerGroupMember();
                groupmember.BuyerGroupMemberId = model.BuyerGroupMemberId;
                groupmember.BuyerGroupId = model.BuyerGroupId;
                groupmember.GroupMember = model.GroupMember;
                obj.MPRBuyerGroupMembers.Add(groupmember);
                obj.SaveChanges();
                int memberid = groupmember.BuyerGroupMemberId;
                status.Sid = memberid;
                return status;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<statuscheckmodel> InsertMPRApprover(MPRApproverModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = new MPRApprover();
                data.EmployeeNo = model.EmployeeNo;
                data.BoolActive = true;
                obj.MPRApprovers.Add(data);
                obj.SaveChanges();
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> InsertMprApprovers(MPRApproverModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = new MPRApprover();
                data.EmployeeNo = model.EmployeeNo;
                data.DeactivatedBy = model.DeactivatedBy;
                data.DeactivatedOn = model.DeactivatedOn;
                obj.MPRApprovers.Add(data);
                obj.SaveChanges();
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> UpdateMprApprovers(MPRApproverModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = obj.MPRApprovers.Where(x => x.EmployeeNo == model.EmployeeNo).FirstOrDefault();
                data.BoolActive = model.BoolActive;
                data.DeactivatedBy = model.DeactivatedBy;
                data.DeactivatedOn = model.DeactivatedOn;
                obj.MPRApprovers.Add(data);
                obj.SaveChanges();
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> InsertMprDepartMents(MPRDepartmentModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = new MPRDepartment();
                data.Department = model.Department;
                data.SecondApprover = model.SecondApprover;
                data.ThirdApprover = model.ThirdApprover;
                obj.MPRDepartments.Add(data);
                obj.SaveChanges();
                int deptid = data.DepartmentId;
                status.Sid = deptid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> UpdateMprDepartMents(MPRDepartmentModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = obj.MPRDepartments.Where(x => x.DepartmentId == model.DepartmentId).FirstOrDefault();
                data.Department = model.Department;
                data.SecondApprover = model.SecondApprover;
                data.ThirdApprover = model.ThirdApprover;
                data.BoolInUse = model.BoolInUse;
                obj.SaveChanges();
                int deptid = data.DepartmentId;
                status.Sid = deptid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> InsertMprProcurement(MPRProcurementSourceModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            var data = new MPRProcurementSource();
            try
            {
                if (model.ProcurementSourceId == 0)
                {
                    data.ProcurementSource = model.ProcurementSource;
                    obj.MPRProcurementSources.Add(data);
                    obj.SaveChanges();
                }
                else
                {
                    var mprdata = obj.MPRProcurementSources.Where(x => x.ProcurementSourceId == model.ProcurementSourceId).FirstOrDefault();
                    mprdata.ProcurementSource = model.ProcurementSource;
                    mprdata.BoolInUse = model.BoolInUse;
                    obj.SaveChanges();
                }

                int pid = data.ProcurementSourceId;
                status.Sid = pid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> InsertGlobalgroup(GlobalGroupModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = new GlobalGroup();
                data.GlobalGroupDescription = model.GlobalGroupDescription;
                obj.GlobalGroups.Add(data);
                obj.SaveChanges();
                int pid = data.GlobalGroupId;
                status.Sid = pid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> InsertGlobalgroupEmployee(GlobalGroupEmployeeModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = new GlobalGroupEmployee();
                data.GlobalGroupId = model.GlobalGroupId;
                data.EmployeeNo = model.EmployeeNo;
                data.UpdatedOn = model.UpdatedOn;
                obj.GlobalGroupEmployees.Add(data);
                obj.SaveChanges();

                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> InsertMPRDispatchLocations(MPRDispatchLocationModel model)
        {
            statuscheckmodel status = new statuscheckmodel();

            try
            {
                var data = new MPRDispatchLocation();
                if (model.DispatchLocationId == 0)
                {
                    data.DispatchLocation = model.DispatchLocation;
                    data.XOrder = model.XOrder;
                    obj.MPRDispatchLocations.Add(data);
                    obj.SaveChanges();
                }
                else
                {
                    var mprlocation = obj.MPRDispatchLocations.Where(x => x.DispatchLocationId == model.DispatchLocationId).FirstOrDefault();
                    mprlocation.DispatchLocation = model.DispatchLocation;
                    mprlocation.XOrder = model.XOrder;
                    mprlocation.BoolInUse = model.BoolInUse;
                    obj.SaveChanges();
                }
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> InsertMPRDocumentationDescription(MPRDocumentationDescriptionModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = new MPRDocumentationDescription();
                if (model.DocumentationDescriptionId == 0)
                {
                    data.DocumentationDescription = model.DocumentationDescription;
                    obj.MPRDocumentationDescriptions.Add(data);
                    obj.SaveChanges();
                }
                else
                {

                }
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> InsertMPRCustomDuty(MPRCustomsDutyModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = new MPRCustomsDuty();
                data.CustomsDutyId = model.CustomsDutyId;
                data.CustomsDuty = model.CustomsDuty;
                data.BoolInUse = true;
                obj.SaveChanges();
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<MPRBuyerGroupModel> GetMPRBuyerGroupsById(int id)
        {
            MPRBuyerGroupModel model = new MPRBuyerGroupModel();
            try
            {
                var data = obj.MPRBuyerGroups.SqlQuery("select * from  MPRBuyerGroups where BuyerGroupId=@id and BoolInUse=1", new SqlParameter("@id", id)).FirstOrDefault();
                model.BuyerGroup = data.BuyerGroup;
                model.BoolInUse = data.BoolInUse;
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<MPRBuyerGroupModel>> GetAllMPRBuyerGroups()
        {
            List<MPRBuyerGroupModel> model = new List<MPRBuyerGroupModel>();
            try
            {
                var data = obj.MPRBuyerGroups.SqlQuery("select * from  MPRBuyerGroups where  BoolInUse=1");
                model = data.Select(x => new MPRBuyerGroupModel()
                {
                    BuyerGroup = x.BuyerGroup,
                    BuyerGroupId = x.BuyerGroupId,
                    BoolInUse = x.BoolInUse
                }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<MPRApproverModel> GetMPRApprovalsById(int id)
        {
            MPRApproverModel model = new MPRApproverModel();
            try
            {
                var data = obj.MPRApprovers.SqlQuery("select * from  MPRApprovers where EmployeeNo=@id and BoolActive=1", new SqlParameter("@id", id)).ToList();
                model = data.Select(x => new MPRApproverModel()
                {
                    EmployeeNo = x.EmployeeNo,
                    DeactivatedBy = x.DeactivatedBy,
                    DeactivatedOn = x.DeactivatedOn
                }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<MPRApproverModel>> GetAllMPRApprovals()
        {
            List<MPRApproverModel> model = new List<MPRApproverModel>();
            try
            {
                var data = obj.MPRApprovers.SqlQuery("select * from  MPRApprovers where  BoolInUse=1");
                model = data.Select(x => new MPRApproverModel()
                {
                    EmployeeNo = x.EmployeeNo,
                    DeactivatedBy = x.DeactivatedBy,
                    DeactivatedOn = x.DeactivatedOn
                }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<MPRDepartmentModel>> GetAllMPRDepartments()
        {
            List<MPRDepartmentModel> model = new List<MPRDepartmentModel>();
            try
            {
                var data = obj.MPRDepartments.SqlQuery("select * from  MPRDepartments where  BoolInUse=1 order by Department");
                model = data.Select(x => new MPRDepartmentModel()
                {
                    DepartmentId = x.DepartmentId,
                    Department = x.Department,
                    SecondApprover = x.SecondApprover,
                    ThirdApprover = x.ThirdApprover
                }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<MPRDepartmentModel> GetMPRDepartmentById(int id)
        {
            MPRDepartmentModel model = new MPRDepartmentModel();
            try
            {
                var data = obj.MPRDepartments.SqlQuery("select * from  MPRDepartments where DepartmentId=@id and BoolActive=1", new SqlParameter("@id", id)).ToList();
                model = data.Select(x => new MPRDepartmentModel()
                {
                    DepartmentId = x.DepartmentId,
                    Department = x.Department,
                    SecondApprover = x.SecondApprover,
                    ThirdApprover = x.ThirdApprover
                }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<MPRDispatchLocationModel>> GetAllMPRDispatchLocations()
        {
            List<MPRDispatchLocationModel> model = new List<MPRDispatchLocationModel>();
            try
            {
                var data = obj.MPRDispatchLocations.SqlQuery("select * from  MPRDepartments where  BoolInUse=1");
                model = data.Select(x => new MPRDispatchLocationModel()
                {
                    DispatchLocationId = x.DispatchLocationId,
                    DispatchLocation = x.DispatchLocation,
                    XOrder = x.XOrder,
                    BoolInUse = x.BoolInUse
                }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<MPRDispatchLocationModel> GetMPRDispatchLocationById(int id)
        {
            MPRDispatchLocationModel model = new MPRDispatchLocationModel();
            try
            {
                var data = obj.MPRDispatchLocations.SqlQuery("select * from  MPRDispatchLocations where DispatchLocationId=@id and BoolActive=1", new SqlParameter("@id", id)).ToList();
                model = data.Select(x => new MPRDispatchLocationModel()
                {
                    DispatchLocationId = x.DispatchLocationId,
                    DispatchLocation = x.DispatchLocation,
                    XOrder = x.XOrder,
                    BoolInUse = x.BoolInUse
                }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<MPRCustomsDutyModel>> GetAllCustomDuty()
        {
            List<MPRCustomsDutyModel> model = new List<MPRCustomsDutyModel>();
            try
            {
                var data = obj.MPRCustomsDuties.SqlQuery("select * from  MPRCustomsDuty where  BoolActive=1");
                model = data.Select(x => new MPRCustomsDutyModel()
                {
                    CustomsDutyId = x.CustomsDutyId,
                    CustomsDuty = x.CustomsDuty,
                    BoolInUse = x.BoolInUse
                }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> InsertYILTerms(YILTermsandConditionModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = new YILTermsandCondition();
                if (model != null)
                {
                    data.TermGroupId = model.TermGroupId;
                    data.BuyerGroupId = model.BuyerGroupId;
                    data.Terms = model.Terms;
                    data.DisplayOrder = model.DisplayOrder;
                    data.DefaultSelect = true;
                    data.CreatedBy = model.CreatedBy;
                    data.CreatedDate = model.CreatedDate;
                    data.DeletedBy = model.DeletedBy;
                    data.DeletedDate = model.DeletedDate;
                }
                obj.YILTermsandConditions.Add(data);
                obj.SaveChanges();
                int termsid = data.TermId;
                status.Sid = termsid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> InsertYILTermsGroup(YILTermsGroupModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = new YILTermsGroup();
                if (model != null)
                {
                    data.TermGroup = model.TermGroup;
                    data.CreatedBy = model.CreatedBy;
                    data.CreatedDate = model.CreatedDate;
                    data.DeletedBy = model.DeletedBy;
                    data.DeletedDate = model.DeletedDate;
                }
                obj.YILTermsGroups.Add(data);
                obj.SaveChanges();
                int termgroupid = data.TermGroupId;
                status.Sid = termgroupid;
                return status;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<statuscheckmodel> InsertRFQTerms(RFQTermsModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var remoteterm = new RemoteRfqTerm();
                vscm.Database.Connection.Open();
                if (model != null)
                {
                    remoteterm.RfqRevisionId = model.RFQrevisionId;
                    remoteterm.termsid = model.termsid;
                    remoteterm.VendorResponse = model.VendorResponse;
                    remoteterm.TermGroup = model.TermGroup;
                    remoteterm.Remarks = model.Remarks;
                    remoteterm.Terms = model.Terms;
                    remoteterm.CreatedBy = model.CreatedBy;
                    remoteterm.CreatedDate = model.CreatedDate;
                    remoteterm.UpdatedBy = model.UpdatedBy;
                    remoteterm.UpdatedDate = model.UpdatedDate;
                    remoteterm.DeletedBy = model.DeletedBy;
                    remoteterm.DeletedDate = model.DeletedDate;
                    //remoteterm.SyncStatus = true;
                }
                vscm.RemoteRfqTerms.Add(remoteterm);
                vscm.SaveChanges();
                vscm.Database.Connection.Close();
                int termsid = remoteterm.VRfqTermsid;

                var rfqterm = new RFQTerm();
                //obj.Database.Connection.Open();
                if (model != null)
                {
                    rfqterm.RfqTermsid = termsid;
                    rfqterm.RFQrevisionId = model.RFQrevisionId;
                    rfqterm.termsid = model.termsid;
                    rfqterm.VendorResponse = model.VendorResponse;
                    //rfqterm.TermGroup = model.TermGroup;
                    rfqterm.Remarks = model.Remarks;
                    rfqterm.CreatedBy = model.CreatedBy;
                    rfqterm.CreatedDate = model.CreatedDate;
                    rfqterm.UpdatedBy = model.UpdatedBy;
                    rfqterm.UpdatedDate = model.UpdatedDate;
                    rfqterm.DeletedBy = model.DeletedBy;
                    rfqterm.DeletedDate = model.DeletedDate;

                }
                obj.RFQTerms.Add(rfqterm);
                obj.SaveChanges();
                status.Sid = termsid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> UpdateRFQTerms(RFQTermsModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var remoteterm = new RemoteRfqTerm();
                vscm.Database.Connection.Open();
                var remotedata = vscm.RemoteRfqTerms.Where(x => x.VRfqTermsid == model.RfqTermsid).FirstOrDefault();
                if (model != null)
                {
                    remotedata.RfqRevisionId = model.RFQrevisionId;
                    remotedata.termsid = (int)model.termsid;
                    remotedata.VendorResponse = model.VendorResponse;
                    remotedata.TermGroup = model.TermGroup;
                    remotedata.Remarks = model.Remarks;
                    remotedata.CreatedBy = model.CreatedBy;
                    remotedata.CreatedDate = (DateTime)model.CreatedDate;
                    remotedata.UpdatedBy = model.UpdatedBy;
                    remotedata.UpdatedDate = model.UpdatedDate;
                    remotedata.DeletedBy = model.DeletedBy;
                    remotedata.DeletedDate = model.DeletedDate;
                    vscm.SaveChanges();
                }
                vscm.Database.Connection.Close();

                obj.Database.Connection.Open();
                var localdata = obj.RFQTerms.Where(x => x.RfqTermsid == remotedata.VRfqTermsid).FirstOrDefault();
                if (model != null)
                {
                    localdata.RFQrevisionId = model.RFQrevisionId;
                    localdata.termsid = model.termsid;
                    localdata.VendorResponse = model.VendorResponse;
                    localdata.Remarks = model.Remarks;
                    localdata.CreatedBy = model.CreatedBy;
                    localdata.CreatedDate = model.CreatedDate;
                    localdata.UpdatedBy = model.UpdatedBy;
                    localdata.UpdatedDate = model.UpdatedDate;
                    localdata.DeletedBy = model.DeletedBy;
                    localdata.DeletedDate = model.DeletedDate;
                    obj.SaveChanges();
                }
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<YILTermsandConditionModel> GetYILTermsByBuyerGroupID(int id)
        {
            YILTermsandConditionModel model = new YILTermsandConditionModel();
            try
            {
                var data = from x in obj.YILTermsandConditions where x.BuyerGroupId == id select x;
                //var data = obj.YILTermsandConditions.SqlQuery("select * from YILTermsandConditions where BuyerGroupId={0} and DeleteFlag=0", id);
                model = data.Select(x => new YILTermsandConditionModel()
                {
                    TermGroupId = x.TermGroupId,
                    Terms = x.Terms,
                    DisplayOrder = x.DisplayOrder,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    DeletedBy = x.DeletedBy,
                    DeletedDate = x.DeletedDate
                }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<YILTermsGroupModel> GetYILTermsGroupById(int id)
        {
            YILTermsGroupModel model = new YILTermsGroupModel();
            try
            {
                var data = from x in obj.YILTermsGroups where x.TermGroupId == id && x.DeleteFlag == false select x;
                //var data = obj.YILTermsGroups.SqlQuery("select * from YILTermsGroup where TermGroupId={0} and DeleteFlag=0", id);
                model = data.Select(x => new YILTermsGroupModel()
                {
                    TermGroupId = x.TermGroupId,
                    TermGroup = x.TermGroup,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    DeletedBy = x.DeletedBy,
                    DeletedDate = x.DeletedDate
                }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<RFQTermsModel> GetRfqTermsById(int termsid)
        {
            RFQTermsModel model = new RFQTermsModel();
            try
            {
                var data = obj.RFQTerms.Where(x => x.RfqTermsid == termsid && x.DeleteFlag == false).FirstOrDefault();
                model.termsid = data.termsid;
                model.UpdatedBy = data.UpdatedBy;
                model.UpdatedDate = data.UpdatedDate;
                model.VendorResponse = data.VendorResponse;
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;
                model.DeletedBy = data.DeletedBy;
                model.DeletedDate = data.DeletedDate;
                model.Remarks = data.Remarks;
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<RFQMasterModel>> GetRfqByVendorId(int vendorid)
        {
            List<RFQMasterModel> model = new List<RFQMasterModel>();
            try
            {
                var data = vscm.RemoteRFQMasters.Where(x => x.VendorId == vendorid && x.DeleteFlag == false).Include(x => x.RemoteRFQRevisions_N).ToList();
                if (data != null)
                {
                    model = data.Select(x => new RFQMasterModel()
                    {
                        RfqNo = x.RFQNo,
                        RfqMasterId = x.RfqMasterId,
                        MPRRevisionId = (int)x.MPRRevisionId,
                        CreatedBy = x.CreatedBy,
                        Revision = x.RemoteRFQRevisions_N.Select(y => new RfqRevisionModel()
                        {
                            RfqRevisionNo = y.RevisionNo,
                            RfqRevisionId = y.rfqRevisionId,
                            RfqValidDate = y.RFQValidDate,
                            CreatedBy = y.CreatedBy,
                            PackingForwading = y.PackingForwarding,
                            ExciseDuty = y.ExciseDuty,
                            salesTax = y.SalesTax,
                            freight = y.Freight
                        }).ToList()

                    }).ToList();


                    //var revisions = vscm.RemoteRFQRevisions.Where(x => x.rfqMasterId == data.RfqMasterId).ToList();
                    //RfqRevisionModel revisionmodel = new RfqRevisionModel();
                    //foreach (var item in revisions)
                    //{
                    //    revisionmodel.RfqRevisionId = item.rfqRevisionId;
                    //    revisionmodel.RfqValidDate = item.RFQValidDate;
                    //    revisionmodel.RfqRevisionNo = item.RevisionNo;
                    //    model.Revision.Add(revisionmodel);
                    //}
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<RFQListView> getRFQList(rfqFilterParams rfqfilterparams)
        {
            List<RFQListView> mprRevisionDetails;
            using (var db = new YSCMEntities()) //ok
            {
                obj.Configuration.ProxyCreationEnabled = false;
                int vendorId = Convert.ToInt32(rfqfilterparams.venderid);
                if (!string.IsNullOrEmpty(rfqfilterparams.typeOfFilter))
                {
                    mprRevisionDetails = obj.RFQListViews.Where(li => li.RFQValidDate <= rfqfilterparams.ToDate && li.RFQValidDate >= rfqfilterparams.FromDate).ToList();
                    if (!string.IsNullOrEmpty(rfqfilterparams.RFQNo))
                        mprRevisionDetails = obj.RFQListViews.Where(li => li.RFQValidDate <= rfqfilterparams.ToDate && li.RFQValidDate >= rfqfilterparams.FromDate && li.RFQNo == rfqfilterparams.RFQNo).ToList();
                    if (!string.IsNullOrEmpty(rfqfilterparams.venderid))
                        mprRevisionDetails = obj.RFQListViews.Where(li => li.RFQValidDate <= rfqfilterparams.ToDate && li.RFQValidDate >= rfqfilterparams.FromDate && li.VendorId == vendorId).ToList();
                    if (!string.IsNullOrEmpty(rfqfilterparams.DocumentNo))
                        mprRevisionDetails = obj.RFQListViews.Where(li => li.RFQValidDate <= rfqfilterparams.ToDate && li.RFQValidDate >= rfqfilterparams.FromDate && li.DocumentNo == rfqfilterparams.DocumentNo).ToList();
                }
                else
                {
                    mprRevisionDetails = obj.RFQListViews.Where(li => li.CreatedDate <= rfqfilterparams.ToDate && li.CreatedDate >= rfqfilterparams.FromDate).ToList();
                    if (!string.IsNullOrEmpty(rfqfilterparams.RFQNo))
                        mprRevisionDetails = obj.RFQListViews.Where(li => li.CreatedDate <= rfqfilterparams.ToDate && li.CreatedDate >= rfqfilterparams.FromDate && li.RFQNo == rfqfilterparams.RFQNo).ToList();
                    if (!string.IsNullOrEmpty(rfqfilterparams.venderid))
                        mprRevisionDetails = obj.RFQListViews.Where(li => li.CreatedDate <= rfqfilterparams.ToDate && li.CreatedDate >= rfqfilterparams.FromDate && li.VendorId == vendorId).ToList();
                    if (!string.IsNullOrEmpty(rfqfilterparams.DocumentNo))
                        mprRevisionDetails = obj.RFQListViews.Where(li => li.CreatedDate <= rfqfilterparams.ToDate && li.CreatedDate >= rfqfilterparams.FromDate && li.DocumentNo == rfqfilterparams.DocumentNo).ToList();
                }
            }
            return mprRevisionDetails;
        }

        public async Task<statuscheckmodel> InsertPAAuthorizationLimits(PAAuthorizationLimitModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = new PAAuthorizationLimit();
                if (model != null)
                {
                    data.DeptId = model.DeptId;
                    data.AuthorizationType = model.AuthorizationType;
                    data.MinPAValue = model.MinPAValue;
                    data.MaxPAValue = model.MaxPAValue;
                    data.CreatedBy = model.CreatedBy;
                    data.CreatedDate = System.DateTime.Now;
                    data.DeletedBY = model.DeletedBY;
                    data.DeletedDate = model.DeletedDate;
                }
                obj.PAAuthorizationLimits.Add(data);
                obj.SaveChanges();
                int id = data.Authid;
                status.Sid = id;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> BulkInsertPAAuthorizationLimits(List<PAAuthorizationLimitModel> model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = new PAAuthorizationLimit();
                foreach (var item in model)
                {
                    data.AuthorizationType = item.AuthorizationType;
                    data.DeptId = item.DeptId;
                    data.MinPAValue = item.MinPAValue;
                    data.MaxPAValue = item.MaxPAValue;
                    data.CreatedBy = item.CreatedBy;
                    data.CreatedDate = item.CreatedDate;
                    obj.PAAuthorizationLimits.Add(data);
                    obj.SaveChanges();
                }
                int id = data.Authid;
                status.Sid = id;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<PAAuthorizationLimitModel> GetPAAuthorizationLimitById(int deptid)
        {
            PAAuthorizationLimitModel model = new PAAuthorizationLimitModel();
            try
            {
                var data = obj.PAAuthorizationLimits.Where(x => x.Authid == deptid && x.DeleteFlag == false).FirstOrDefault();
                model.DeptId = data.DeptId;
                model.MaxPAValue = data.MaxPAValue;
                model.MinPAValue = data.MinPAValue;
                model.AuthorizationType = data.AuthorizationType;
                model.DeletedBY = data.DeletedBY;
                model.DeletedDate = data.DeletedDate;
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> CreatePAAuthirizationEmployeeMapping(PAAuthorizationEmployeeMappingModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var mapping = new PAAuthorizationEmployeeMapping();
                if (model != null)
                {
                    mapping.Authid = model.Authid;
                    mapping.FunctionalRoleId = model.FunctionalRoleId;
                    mapping.CreatedBY = model.CreatedBY;
                    mapping.CreatedDate = System.DateTime.Now;
                    mapping.Employeeid = model.Employeeid;
                    mapping.LessBudget = model.LessBudget;
                    mapping.MoreBudget = model.MoreBudget;
                    mapping.DeletedBy = model.DeletedBy;
                    mapping.DeletedDate = model.DeletedDate;
                }
                obj.PAAuthorizationEmployeeMappings.Add(mapping);
                obj.SaveChanges();
                int mapid = mapping.PAmapid;
                status.Sid = mapid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<PAAuthorizationEmployeeMappingModel> GetMappingEmployee(PAAuthorizationLimitModel limit)
        {
            PAAuthorizationEmployeeMappingModel model = new PAAuthorizationEmployeeMappingModel();
            try
            {
                var authdata = obj.PAAuthorizationLimits.Where(x => x.Authid == limit.Authid && x.MinPAValue >= limit.MinPAValue && x.MaxPAValue <= limit.MaxPAValue).FirstOrDefault();
                var mappingdata = obj.PAAuthorizationEmployeeMappings.Where(x => x.Authid == authdata.Authid && x.DeleteFlag == false).FirstOrDefault();
                var employeedata = obj.Employees.Where(x => x.EmployeeNo == mappingdata.Employeeid).FirstOrDefault();
                model.Employeeid = mappingdata.Employeeid;
                model.Employeename = employeedata.Name;
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> CreatePACreditDaysmaster(PACreditDaysMasterModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            var credit = new PACreditDaysMaster();
            try
            {
                if (model != null)
                {
                    credit.MinDays = model.MinDays;
                    credit.MaxDays = model.MaxDays;
                    credit.DeletedBy = model.DeletedBy;
                    credit.DeletedDate = model.DeletedDate;
                    credit.CreatedBy = model.CreatedBy;
                    credit.CreatedDate = System.DateTime.Now;
                }
                obj.PACreditDaysMasters.Add(credit);
                obj.SaveChanges();
                int creditid = credit.CreditDaysid;
                status.Sid = creditid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<PACreditDaysMasterModel> GetCreditdaysMasterByID(int creditdaysid)
        {
            PACreditDaysMasterModel model = new PACreditDaysMasterModel();
            try
            {
                var creditdata = obj.PACreditDaysMasters.Where(x => x.CreditDaysid == creditdaysid).FirstOrDefault();
                if (creditdata != null)
                {
                    model.CreditDaysid = creditdata.CreditDaysid;
                    model.MinDays = creditdata.MinDays;
                    model.MaxDays = creditdata.MaxDays;
                    model.CreatedBy = creditdata.CreatedBy;
                    model.CreatedDate = creditdata.CreatedDate;
                    model.DeletedBy = creditdata.DeletedBy;
                    model.DeletedDate = creditdata.DeletedDate;
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> AssignCreditdaysToEmployee(PACreditDaysApproverModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                ////var data = obj.PAAuthorizationLimits.Where(x => x.Authid == model.AuthId).FirstOrDefault();
                var creditapprover = new PACreditDaysApprover();
                creditapprover.AuthId = model.AuthId;
                creditapprover.EmployeeNo = model.EmployeeNo;
                creditapprover.CreditdaysId = Convert.ToByte(model.CreditdaysId);
                creditapprover.Createdby = model.Createdby;
                creditapprover.CreatedDate = System.DateTime.Now;
                creditapprover.DeletedBy = model.DeletedBy;
                creditapprover.DeletedDate = model.DeletedDate;
                obj.PACreditDaysApprovers.Add(creditapprover);
                obj.SaveChanges();
                int approvalid = creditapprover.CRApprovalId;
                status.Sid = approvalid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> RemovePAAuthorizationLimitsByID(int authid)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var removedata = obj.PAAuthorizationLimits.Where(x => x.Authid == authid && x.DeleteFlag == false).FirstOrDefault();
                if (removedata != null)
                {
                    removedata.DeleteFlag = true;
                    obj.SaveChanges();
                }
                var mappingdata = obj.PAAuthorizationEmployeeMappings.Where(x => x.Authid == removedata.Authid && x.DeleteFlag == false).ToList();
                if (mappingdata != null)
                {
                    foreach (var item in mappingdata)
                    {
                        item.DeleteFlag = true;
                        obj.SaveChanges();
                    }
                }
                int id = removedata.Authid;
                status.Sid = id;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> RemovePACreditDaysMaster(int creditid)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var creditdata = obj.PACreditDaysMasters.Where(x => x.CreditDaysid == creditid && x.DeleteFlag == false).FirstOrDefault();
                if (creditdata != null)
                {
                    creditdata.DeleteFlag = true;
                    obj.SaveChanges();
                }
                int id = creditdata.CreditDaysid;
                status.Sid = id;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<PAAuthorizationLimitModel>> GetPAAuthorizationLimitsByDeptId(int departmentid)
        {
            List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
            try
            {
                model = obj.PAAuthorizationLimits.Where(x => x.DeptId == departmentid && x.DeleteFlag == false).Select(x => new PAAuthorizationLimitModel
                {
                    MinPAValue = x.MinPAValue,
                    MaxPAValue = x.MaxPAValue
                }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> RemovePACreditDaysApprover(EmployeemappingtocreditModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = obj.PACreditDaysApprovers.Where(x => x.CRApprovalId == model.CRApprovalId).FirstOrDefault();
                if (data != null)
                {
                    data.DeleteFlag = true;
                    obj.SaveChanges();
                }
                status.Sid = data.AuthId;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> RemovePurchaseApprover(EmployeemappingtopurchaseModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = obj.PAAuthorizationEmployeeMappings.Where(x => x.PAmapid == model.PAmapid).FirstOrDefault();
                if (data != null)
                {
                    data.DeleteFlag = true;
                    obj.SaveChanges();
                }
                status.Sid = data.Authid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<PACreditDaysApproverModel> GetPACreditDaysApproverById(int ApprovalId)
        {
            PACreditDaysApproverModel model = new PACreditDaysApproverModel();
            try
            {
                var data = obj.PACreditDaysApprovers.Where(x => x.CRApprovalId == ApprovalId).FirstOrDefault();
                if (data != null)
                {
                    model.AuthId = data.AuthId;
                    model.EmployeeNo = data.EmployeeNo;
                    model.CreditdaysId = data.CreditdaysId;
                    model.Createdby = data.Createdby;
                    model.CreatedDate = data.CreatedDate;
                    model.DeletedBy = data.DeletedBy;
                    model.DeletedDate = data.DeletedDate;
                    model.CRApprovalId = data.CRApprovalId;
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //public async Task<List<EmployeModel>> GetEmployeeMappings(PAConfigurationModel model)
        //{
        //    List<EmployeModel> employee = new List<EmployeModel>();
        //    model.PAValue = model.UnitPrice;
        //    if (model.PAValue > model.TargetSpend)
        //    {
        //        model.Budgetvalue = false;
        //    }
        //    else
        //    {
        //        model.Budgetvalue = true;
        //    }
        //    try
        //    {
        //        if (model != null)
        //        {
        //            //var padata = obj.PAAuthorizationLimits.Where(x => x.MinPAValue >= model.PAValue && x.MaxPAValue <= model.PAValue).FirstOrDefault();
        //            var padata = obj.PAAuthorizationLimits.Where(x => x.MinPAValue.CompareTo(model.PAValue) <= 0 && x.MaxPAValue.CompareTo(model.PAValue) >= 0 && x.DeptId==model.DeptId).FirstOrDefault();
        //            if (padata != null)
        //            {
        //                //string.Equals(padata.AuthorizationType,"pa", StringComparison.CurrentCultureIgnoreCase);
        //                //padata.AuthorizationType.Contains( StringComparison.CurrentCultureIgnoreCase);
        //                if (padata.AuthorizationType.ToLower().Equals("pa"))
        //                {
        //                    var mappingdata = obj.PAAuthorizationEmployeeMappings.Where(x => x.Authid == padata.Authid).FirstOrDefault();
        //                    if (mappingdata != null)
        //                    {
        //                        var employeedata = obj.Employees.Where(x => x.EmployeeNo == mappingdata.Employeeid).ToList();
        //                        employee = employeedata.Select(x => new EmployeModel()
        //                        {
        //                            EmployeeNo = x.EmployeeNo,
        //                            Name = x.Name
        //                        }).ToList();
        //                    }
        //                }
        //                else
        //                {
        //                    var creditdata = obj.PACreditDaysApprovers.Where(x => x.AuthId == padata.Authid).FirstOrDefault();
        //                    var employeedata = obj.Employees.Where(x => x.EmployeeNo == creditdata.EmployeeNo).ToList();
        //                    if (creditdata != null)
        //                    {
        //                        var creditmasterdata = obj.PACreditDaysMasters.Where(x => x.CreditDaysid == creditdata.CreditdaysId).FirstOrDefault();
        //                    }
        //                    employee = employeedata.Select(x => new EmployeModel()
        //                    {
        //                        EmployeeNo = x.EmployeeNo,
        //                        Name = x.Name
        //                    }).ToList();
        //                }
        //            }
        //            else
        //            {
        //                return employee;
        //            }

        //        }
        //        return employee;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        public async Task<EmployeModel> GetEmployeeMappings(PAConfigurationModel model)
        {
            EmployeModel employee = new EmployeModel();
            model.PAValue = model.UnitPrice;
            if (model.PAValue > model.TargetSpend)
            {
                model.LessBudget = false;
                model.MoreBudget = true;
            }
            else
            {
                model.MoreBudget = false;
                model.LessBudget = true;
            }
            string Termscode = model.PaymentTermCode.Substring(model.PaymentTermCode.Length - 2, 2);
            try
            {
                var BuyerManagers = obj.LoadBuyerManagers.Where(x => model.MPRItemDetailsid.Contains(x.Itemdetailsid) && x.BoolValidRevision == true).FirstOrDefault();
                if (BuyerManagers != null)
                {
                    employee.BuyerGroupManager = BuyerManagers.EmployeeName;
                    employee.BuyerGroupNo = BuyerManagers.EmpNo;
                    employee.BGRole = BuyerManagers.Role;
                }
                var projectmanagers = obj.LoadProjectManagers.Where(x => model.MPRItemDetailsid.Contains(x.Itemdetailsid) && x.BoolValidRevision == true).FirstOrDefault();
                if (projectmanagers != null)
                {
                    employee.ProjectManager = projectmanagers.EmployeeName;
                    employee.ProjectMangerNo = projectmanagers.EmpNo;
                    employee.PMRole = projectmanagers.Role;
                }
                var PAandCRmapping = obj.PAandCRMappings.Where(x => x.DepartmentId == model.DeptId && x.minpavalue.CompareTo(model.PAValue) <= 0 && x.maxpavalue.CompareTo(model.PAValue) >= 0 && x.morebudget == model.MoreBudget && x.lessbudget == model.LessBudget).OrderBy(x=>x.roleorder).ToList();
                employee.Approvers = PAandCRmapping.Select(x => new PurchaseCreditApproversModel()
                {
                    ApproverName = x.Name,
                    AuthorizationType = x.AuthorizationType,
                    RoleName = x.role,
                    EmployeeNo = x.EmployeeNo,
                    RoleId = x.FunctionalRoleId
                }).ToList();

                return employee;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //public async Task<EmployeeModel> GetEmployeeMappingsList(List<PAConfigurationModel> model)
        //{
        //    EmployeeModel employee = new EmployeeModel();
        //    model.PAValue = model.UnitPrice;
        //    try
        //    {
        //        if (model != null)
        //        {
        //            //var padata = obj.PAAuthorizationLimits.Where(x => x.MinPAValue >= model.PAValue && x.MaxPAValue <= model.PAValue).FirstOrDefault();
        //            var padata = obj.PAAuthorizationLimits.Where(x => x.MinPAValue.CompareTo(model.PAValue) <= 0 && x.MaxPAValue.CompareTo(model.PAValue) >= 0 && x.DeptId == model.DeptId).FirstOrDefault();
        //            if (padata != null)
        //            {
        //                var mappingdata = obj.PAAuthorizationEmployeeMappings.Where(x => x.Authid == padata.Authid && x.LessBudget == true).FirstOrDefault();
        //                if (mappingdata != null)
        //                {
        //                    var res = obj.PACreditDaysApprovers.ToList();
        //                    var creditapprovaldata = obj.PACreditDaysApprovers.Where(x => x.AuthId == padata.Authid && x.EmployeeNo == mappingdata.Employeeid).FirstOrDefault();
        //                    if (creditapprovaldata != null)
        //                    {
        //                        var creditmasterdata = obj.PACreditDaysMasters.Where(x => x.CreditDaysid == creditapprovaldata.CreditdaysId).FirstOrDefault();
        //                    }
        //                    var employeedata = obj.Employees.Where(x => x.EmployeeNo == creditapprovaldata.EmployeeNo).ToList();
        //                    foreach (var item in employeedata)
        //                    {
        //                        employee.EmployeeNo = item.EmployeeNo;
        //                        employee.Name = item.Name;
        //                    }
        //                }
        //            }
        //        }
        //        return employee;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        public List<LoadItemsByID> GetItemsByMasterIDs(PADetailsModel masters)
        {
            //List<LoadItemsByID> view = new List<LoadItemsByID>();
            try
            {
                using (YSCMEntities yscm = new YSCMEntities())
                {
                    var sqlquery = "";
                    sqlquery = "select * from LoadItemsByID where Status='Approved'";
                    if (masters.VendorId != 0)
                        sqlquery += " and VendorId='" + masters.VendorId + "'";
                    if (masters.RevisionId != 0)
                        sqlquery += " and MPRRevisionId='" + masters.RevisionId + "'";
                    if (masters.RFQNo != null)
                        sqlquery += " and RFQNo='" + masters.RFQNo + "'";
                    if (masters.DocumentNumber != null)
                        sqlquery += " and DocumentNo='" + masters.DocumentNumber + "'";
                    if (masters.BuyerGroupId != 0)
                        sqlquery += " and BuyerGroupId='" + masters.BuyerGroupId + "'";
                    if (masters.SaleOrderNo != null)
                        sqlquery += " and SaleOrderNo='" + masters.SaleOrderNo + "'";
                    if (masters.DeptID != 0)
                        sqlquery += " and DepartmentId='" + masters.DeptID + "'";
                    //if (masters.EmployeeNo != null)
                    //    sqlquery += " and DepartmentId='" + masters.EmployeeNo + "'";
                    if (masters.EmployeeNo != null)
                        sqlquery += " and ProjectManager='" + masters.EmployeeNo + "'";

                    return yscm.Database.SqlQuery<LoadItemsByID>(sqlquery).ToList();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<RfqRevisionModel> GetLocalRfqDetailsById(int revisionId)
        {
            RfqRevisionModel revision = new RfqRevisionModel();
            try
            {
                var localrevision = obj.RFQRevisions_N.Where(x => x.rfqRevisionId == revisionId && x.DeleteFlag == false).Include(x => x.RFQMaster).Include(x => x.RFQItems_N).FirstOrDefault();
                if (localrevision != null)
                {
                    revision.RfqMasterId = localrevision.rfqMasterId;
                    revision.RfqRevisionNo = localrevision.RevisionNo;
                    revision.CreatedBy = localrevision.CreatedBy;
                    revision.CreatedDate = localrevision.CreatedDate;
                    revision.PackingForwading = localrevision.PackingForwarding;
                    revision.salesTax = localrevision.SalesTax;
                    revision.Insurance = localrevision.Insurance;
                    revision.CustomsDuty = localrevision.CustomsDuty;
                    revision.PaymentTermDays = localrevision.PaymentTermDays;
                    revision.PaymentTermRemarks = localrevision.PaymentTermRemarks;
                    revision.BankGuarantee = localrevision.BankGuarantee;
                    revision.DeliveryMaxWeeks = localrevision.DeliveryMaxWeeks;
                    revision.DeliveryMinWeeks = localrevision.DeliveryMinWeeks;


                    var rfqmasters = obj.RFQMasters.Where(x => x.RfqMasterId == localrevision.rfqMasterId).ToList();
                    var masters = new RFQMasterModel();
                    var vendors = new VendormasterModel();
                    if (rfqmasters != null)
                    {
                        foreach (var item in rfqmasters)
                        {
                            masters.RfqMasterId = item.RfqMasterId;
                            masters.RfqNo = item.RFQNo;
                            masters.RfqUniqueNo = item.RFQUniqueNo;
                            masters.VendorId = item.VendorId;
                            var vendormasters = obj.VendorMasters.Where(x => x.Vendorid == masters.VendorId).FirstOrDefault();
                            masters.Vendor = new VendormasterModel()
                            {
                                ContactNo = vendormasters.ContactNo,
                                VendorCode = vendormasters.VendorCode,
                                VendorName = vendormasters.VendorName,
                                Emailid = vendormasters.Emailid,
                                Street = vendormasters.Street,
                            };
                            masters.MPRRevisionId = (int)item.MPRRevisionId;
                            masters.CreatedBy = item.CreatedBy;
                        }
                    }
                    revision.rfqmaster = masters;
                    var vendordata = obj.VendorMasters.Where(x => x.Vendorid == masters.VendorId).FirstOrDefault();
                    if (vendordata != null)
                    {
                        revision.VendorName = vendordata.VendorName;
                    }

                    var rfqitemss = obj.RFQItems.Where(x => x.RFQRevisionId == localrevision.rfqRevisionId).ToList();
                    if (rfqitemss != null)
                    {
                        foreach (var item in rfqitemss)
                        {
                            RfqItemModel rfqitems = new RfqItemModel();
                            rfqitems.HSNCode = item.HSNCode;
                            rfqitems.MRPItemsDetailsID = item.MPRItemDetailsid;
                            rfqitems.QuotationQty = item.QuotationQty;
                            rfqitems.RFQRevisionId = item.RFQRevisionId;
                            rfqitems.RFQItemID = item.RFQItemsId;
                            revision.rfqitem.Add(rfqitems);
                        }
                    }
                }
                var rfqterms = obj.RFQTerms.Where(x => x.RFQrevisionId == revisionId).ToList();

                RFQTermsModel terms = new RFQTermsModel();
                foreach (var item in rfqterms)
                {
                    terms.termsid = item.termsid;
                    terms.RfqTermsid = item.RfqTermsid;
                    terms.Remarks = item.Remarks;
                    terms.VendorResponse = item.VendorResponse;
                    revision.RFQTerms.Add(terms);
                }
                return revision;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<DepartmentModel>> GetAllDepartments()
        {
            List<DepartmentModel> model = new List<DepartmentModel>();
            try
            {
                var departments = obj.Departments.ToList();
                foreach (var item in departments)
                {
                    model.Add(new DepartmentModel()
                    {
                        DepartmentID = item.DepartmentId,
                        DepartmentName = item.DepartmentName
                    });
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<PAAuthorizationLimitModel>> GetSlabsByDepartmentID(int DeptID)
        {
            List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
            try
            {
                var data = obj.PAAuthorizationLimits.Where(x => x.DeptId == DeptID && x.DeleteFlag == false).ToList();
                if (data != null)
                {
                    model = data.Select(x => new PAAuthorizationLimitModel()
                    {
                        MaxPAValue = x.MaxPAValue,
                        MinPAValue = x.MinPAValue,
                        Authid = x.Authid
                    }).ToList();
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<EmployeModel>> GetAllEmployee()
        {
            List<EmployeModel> model = new List<EmployeModel>();
            try
            {
                var data = obj.Employees.ToList();
                if (data != null)
                {
                    model = data.Select(x => new EmployeModel()
                    {
                        EmployeeNo = x.EmployeeNo,
                        Name = x.Name
                    }).ToList();
                }
                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<PAAuthorizationLimitModel>> GetAllCredits()
        {
            List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
            try
            {
                //padata.AuthorizationType.ToLower().Equals("pa")
                var data = obj.PAAuthorizationLimits.Where(x => x.AuthorizationType.ToLower() == "cr").ToList();
                //var mappingdata = obj.PAAuthorizationEmployeeMappings.ToList();
                foreach (var item in data)
                {
                    model.Add(new PAAuthorizationLimitModel()
                    {
                        Authid = item.Authid,
                        MinPAValue = item.MinPAValue,
                        MaxPAValue = item.MaxPAValue,
                        //PAAuthorizationEmployeeMappings=mappingdata.Where(x=>x.Authid== item.Authid).Select(x=>new PAAuthorizationEmployeeMappingModel()
                        //{
                        //    Employeeid=x.Employeeid,
                        //    AuthLevel=x.AuthLevel
                        //}).ToList()
                    });
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<PACreditDaysMasterModel>> GetAllCreditDays()
        {
            List<PACreditDaysMasterModel> model = new List<PACreditDaysMasterModel>();
            try
            {
                var credit = obj.PACreditDaysMasters.ToList();
                if (credit != null)
                {
                    model = credit.Select(x => new PACreditDaysMasterModel()
                    {
                        CreditDaysid = x.CreditDaysid,
                        MinDays = x.MinDays,
                        MaxDays = x.MaxDays
                    }).ToList();
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<MPRPAPurchaseModesModel>> GetAllMprPAPurchaseModes()
        {
            List<MPRPAPurchaseModesModel> model = new List<MPRPAPurchaseModesModel>();
            try
            {
                var data = obj.MPRPAPurchaseModes.Where(x => x.BoolInUse == true).ToList();
                model = data.Select(x => new MPRPAPurchaseModesModel()
                {
                    PurchaseModeId = x.PurchaseModeId,
                    PurchaseMode = x.PurchaseMode,
                    XOrder = x.XOrder
                }).ToList();

                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<MPRPAPurchaseTypesModel>> GetAllMprPAPurchaseTypes()
        {
            List<MPRPAPurchaseTypesModel> model = new List<MPRPAPurchaseTypesModel>();
            try
            {
                var data = obj.MPRPAPurchaseTypes.Where(x => x.BoolInUse == true).ToList();
                model = data.Select(x => new MPRPAPurchaseTypesModel()
                {
                    PurchaseTypeId = x.PurchaseTypeId,
                    PurchaseType = x.PurchaseType,
                    XOrder = x.XOrder
                }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> InsertPurchaseAuthorization(MPRPADetailsModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var authorization = new MPRPADetail();
                if (model != null)
                {
                    authorization.PurchaseTypeId = model.PurchaseTypeId;
                    authorization.PurchaseModeId = model.PurchaseModeId;
                    authorization.RequestedOn = System.DateTime.Now;
                    authorization.RequestedBy = model.RequestedBy;
                    authorization.DepartmentID = model.DepartmentID;
                    authorization.BuyerGroupId = model.BuyerGroupId;
                    authorization.ProjectCode = model.ProjectCode;
                    authorization.ProjectName = model.ProjectName;
                    authorization.PackagingForwarding = model.PackagingForwarding;
                    authorization.Taxes = model.Taxes;
                    authorization.Freight = model.Freight;
                    authorization.Insurance = model.Insurance;
                    authorization.DeliveryCondition = model.DeliveryCondition;
                    authorization.ShipmentMode = model.ShipmentMode;
                    authorization.PaymentTerms = model.PaymentTerms;
                    authorization.CreditDays = model.CreditDays;
                    authorization.Warranty = model.Warranty;
                    authorization.BankGuarantee = model.BankGuarantee;
                    authorization.LDPenaltyTerms = model.LDPenaltyTerms;
                    authorization.SpecialInstructions = model.SpecialInstructions;
                    authorization.FactorsForImports = model.FactorsForImports;
                    authorization.SpecialRemarks = model.SpecialRemarks;
                    authorization.SuppliersReference = model.SuppliersReference;
                    obj.MPRPADetails.Add(authorization);
                    obj.SaveChanges();
                    status.Sid = authorization.PAId;

                    // var itemsdata = obj.RFQItemsInfo_N.Where(x => model.Item.Contains(x.RFQItemsId));
                    foreach (var item in model.Item)
                    {
                        var itemdata = obj.RFQItemsInfo_N.Where(x => x.RFQItemsId == item.RFQItemsId).ToList();
                        foreach (var items in itemdata)
                        {
                            //items.Paid = status.Sid;
                            //obj.SaveChanges();
                            PAItem paitem = new PAItem()
                            {
                                PAID = status.Sid,
                                RfqSplitItemId = items.RFQSplitItemId
                            };
                            obj.PAItems.Add(paitem);
                            obj.SaveChanges();
                        }
                    }
                    //var rfqterms = obj.RFQTerms.Where(x => model.TermId.Contains(x.termsid)).ToList();
                    foreach (var item in model.TermId)
                    {
                        var paterms = new PATerm()
                        {
                            PAID = status.Sid,
                            RfqTermId = item,
                        };
                        obj.PATerms.Add(paterms);
                        obj.SaveChanges();
                    }
                    foreach (var item in model.ApproversList)
                    {
                        var Approveritem = new MPRPAApprover()
                        {
                            PAId = status.Sid,
                            ApproverLevel = 1,
                            RoleName = item.RoleId,
                            Approver = item.EmployeeNo,
                            ApproversRemarks = item.ApproversRemarks,
                            ApprovalStatus = "submitted",
                            ApprovedOn = System.DateTime.Now
                        };
                        obj.MPRPAApprovers.Add(Approveritem);
                        obj.SaveChanges();
                        //var Approveritem1 = new MPRPAApprover()
                        //{
                        //    PAId = status.Sid,
                        //    ApproverLevel = 1,
                        //    RoleName = item.RoleId,
                        //    Approver = item.EmployeeNo,
                        //    ApproversRemarks = item.ApproversRemarks,
                        //    ApprovalStatus = "submitted",
                        //    ApprovedOn = System.DateTime.Now
                        //};
                    }
                }
                else
                {

                }

                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<MPRPADetailsModel> GetMPRPADeatilsByPAID(int PID)
        {
            MPRPADetailsModel model = new MPRPADetailsModel();
            try
            {
                var data = obj.MPRPADetails.Where(x => x.PAId == PID).FirstOrDefault();
                if (data != null)
                {
                    model.PurchaseTypeId = data.PurchaseTypeId;
                    var purchasetype = obj.MPRPAPurchaseTypes.Where(x => x.PurchaseTypeId == model.PurchaseTypeId).FirstOrDefault();
                    model.purchasetypes.PurchaseType = purchasetype.PurchaseType;
                    model.PurchaseModeId = data.PurchaseModeId;
                    var purchasemode = obj.MPRPAPurchaseModes.Where(x => x.PurchaseModeId == model.PurchaseModeId).FirstOrDefault();
                    model.purchasemodes.PurchaseMode = purchasemode.PurchaseMode;
                    model.RequestedOn = data.RequestedOn;
                    model.RequestedBy = data.RequestedBy;
                    model.DepartmentID = data.DepartmentID;
                    var department = obj.MPRDepartments.Where(x => x.DepartmentId == model.DepartmentID).FirstOrDefault();
                    model.department.Department = department.Department;
                    model.BuyerGroupId = data.BuyerGroupId;
                    var buyergroup = obj.MPRBuyerGroups.Where(x => x.BuyerGroupId == model.BuyerGroupId).FirstOrDefault();
                    model.buyergroup.BuyerGroup = buyergroup.BuyerGroup;
                    model.ProjectCode = data.ProjectCode;
                    model.ProjectName = data.ProjectName;
                    model.PackagingForwarding = data.PackagingForwarding;
                    model.Taxes = data.Taxes;
                    model.Freight = data.Freight;
                    model.Insurance = data.Insurance;
                    model.DeliveryCondition = data.DeliveryCondition;
                    model.ShipmentMode = data.ShipmentMode;
                    model.PaymentTerms = data.PaymentTerms;
                    model.CreditDays = data.CreditDays;
                    model.Warranty = data.Warranty;
                    model.BankGuarantee = data.BankGuarantee;
                    model.LDPenaltyTerms = data.LDPenaltyTerms;
                    model.SpecialInstructions = data.SpecialInstructions;
                    model.FactorsForImports = data.FactorsForImports;
                    model.SpecialRemarks = data.SpecialRemarks;
                    model.SuppliersReference = data.SuppliersReference;
                    var statusdata = obj.LoadItemsByIDs.Where(x => x.Status == "Approved" && x.paid == PID).ToList();
                    model.Item = statusdata.Select(x => new RfqItemModel()
                    {
                        ItemDescription = x.ItemDescription,
                        UnitPrice = Convert.ToDecimal(x.UnitPrice),
                        QuotationQty = x.QuotationQty,
                        DocumentNo = x.DocumentNo,
                        SaleOrderNo = x.SaleOrderNo,
                        Department = x.Department,
                        TargetSpend = Convert.ToDecimal(x.TargetSpend),
                        PaymentTermCode = x.PaymentTermCode,
                        VendorName = x.VendorName,
                        DepartmentId = x.DepartmentId,
                        MRPItemsDetailsID = Convert.ToInt16(x.MPRItemDetailsid),
                    }).ToList();
                    var approverdata = obj.GetmprApproverdeatils.Where(x => x.PAId == PID).ToList();
                    model.ApproversList = approverdata.Select(x => new MPRPAApproversModel()
                    {
                        ApproverName = x.Name,
                        RoleName = x.RoleName,
                        ApproversRemarks = x.ApproversRemarks,
                        ApprovalStatus = x.ApprovalStatus,
                        EmployeeNo = x.Approver,
                        ApprovedOn = x.ApprovedOn
                    }).ToList();
                    return model;
                }
                else
                {
                    return model;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<List<MPRPADetailsModel>> GetAllMPRPAList()
        {
            List<MPRPADetailsModel> model = new List<MPRPADetailsModel>();
            try
            {
                var data = obj.MPRPADetails.ToList();
                if (data != null)
                {
                    model = data.Select(c => new MPRPADetailsModel()
                    {
                        PAId = c.PAId,
                        RequestedBy = c.RequestedBy,
                        RequestedOn = c.RequestedOn
                    }).ToList();
                    return model;
                }
                else
                {
                    return model;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<PAFunctionalRolesModel>> GetAllPAFunctionalRoles()
        {
            List<PAFunctionalRolesModel> model = new List<PAFunctionalRolesModel>();
            try
            {
                var roles = obj.PAFunctionalRoles.OrderBy(x => x.XOrder).ToList();
                if (roles != null)
                {
                    model = roles.Select(x => new PAFunctionalRolesModel()
                    {
                        FunctionalRoleId = x.FunctionalRoleId,
                        FunctionalRole = x.FunctionalRole,
                        XOrder = x.XOrder
                    }).ToList();
                }
                else
                {
                    return model;
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<EmployeemappingtocreditModel>> GetCreditSlabsandemployees()
        {
            List<EmployeemappingtocreditModel> model = new List<EmployeemappingtocreditModel>();
            try
            {
                var data = obj.Employeemappingtocredits.OrderByDescending(x => x.CreditdaysId).ToList();
                if (data != null)
                {
                    model = data.Select(x => new EmployeemappingtocreditModel()
                    {
                        Authid = x.Authid,
                        AuthorizationType = x.AuthorizationType,
                        CreditdaysId = x.CreditdaysId,
                        DeptId = x.DeptId,
                        EmployeeNo = x.EmployeeNo,
                        Name = x.Name,
                        MinDays = x.MinDays,
                        MaxDays = x.MaxDays,
                        MinPAValue = x.MinPAValue,
                        MaxPAValue = x.MaxPAValue,
                        CRApprovalId = x.CRApprovalId
                    }).ToList();
                }
                else
                {
                    return model;
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<EmployeemappingtopurchaseModel>> GetPurchaseSlabsandMappedemployees()
        {
            List<EmployeemappingtopurchaseModel> model = new List<EmployeemappingtopurchaseModel>();
            try
            {
                var data = obj.Employeemappingtopurchases.OrderBy(x => x.Authid).ToList();
                if (data != null)
                {
                    model = data.Select(x => new EmployeemappingtopurchaseModel()
                    {
                        Authid = x.Authid,
                        AuthorizationType = x.AuthorizationType,
                        MaxPAValue = x.MaxPAValue,
                        MinPAValue = x.MinPAValue,
                        Employeeid = x.Employeeid,
                        LessBudget = x.LessBudget,
                        MoreBudget = x.MoreBudget,
                        DepartmentName = x.Department,
                        Name = x.Name,
                        FunctionalRoleId = x.FunctionalRoleId,
                        PAmapid = x.PAmapid
                    }).ToList();
                    return model;
                }
                else
                {
                    return model;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<ProjectManagerModel>> LoadAllProjectManagers()
        {
            List<ProjectManagerModel> model = new List<ProjectManagerModel>();
            try
            {
                var data = obj.MPRRevisions.Where(x => x.BoolValidRevision == true).Select(x => x.ProjectManager).Distinct().ToList();
                if (data != null)
                {
                    var vmodel = obj.Employees.Where(x => data.Contains(x.EmployeeNo)).ToList();
                    model = vmodel.Select(x => new ProjectManagerModel()
                    {
                        EmployeeNo = x.EmployeeNo,
                        Name = x.Name
                    }).ToList();
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<VendormasterModel>> LoadVendorByMprDetailsId(List<int?> MPRItemDetailsid)
        {
            List<VendormasterModel> model = new List<VendormasterModel>();
            try
            {
                //var data = obj.LoadItemsByIDs.Where(x => MPRItemDetailsid.Contains(x.MPRItemDetailsid)).ToList();
                var vendor = (from xx in obj.LoadItemsByIDs select xx).Where(y => MPRItemDetailsid.Contains(y.MPRItemDetailsid)).GroupBy(n => new { n.VendorId, n.VendorName }).Select(x => x.FirstOrDefault()).ToList();
                //var data = obj.LoadItemsByIDs.Distinct().Where(x => MPRItemDetailsid.Contains(x.MPRItemDetailsid)).Distinct().ToList();
                model = vendor.Select(x => new VendormasterModel()
                {
                    VendorName = x.VendorName,
                    Vendorid = x.VendorId
                }).ToList();
                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<MPRPAApproversModel>> GetAllApproversList()
        {
            List<MPRPAApproversModel> model = new List<MPRPAApproversModel>();
            try
            {
                var data = obj.MPRPAApprovers.ToList();
                if (data != null)
                {
                    model = data.Select(x => new MPRPAApproversModel()
                    {
                        PAId = x.PAId,
                        ApproverLevel = x.ApproverLevel,
                        RoleName = x.RoleName,
                        ApproverName = x.Approver,
                        ApproversRemarks = x.ApproversRemarks,
                        ApprovalStatus = x.ApprovalStatus,
                        ApprovedOn = x.ApprovedOn
                    }).ToList();
                    return model;
                }
                else
                {
                    return model;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<GetmprApproverdeatil>> GetMprApproverDetailsBySearch(PAApproverDetailsInputModel model)
        {
            List<GetmprApproverdeatil> details = new List<GetmprApproverdeatil>();
            try
            {
                var sqlquery = "";
                sqlquery = "select * from GetmprApproverdeatils where Approver='" + model.CreatedBy + "'";
                if (model.Paid != 0)
                    sqlquery += " and  PAId='" + model.Paid + "'";
                if (model.Status != null)
                    sqlquery += " and ApprovalStatus='" + model.Status + "'";
                if (model.FromDate != null && model.ToDate != null)
                    sqlquery += " and RequestedOn between '" + model.FromDate + "' and '" + model.ToDate + "'";

                details = obj.Database.SqlQuery<GetmprApproverdeatil>(sqlquery).ToList();
                return details;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> UpdateMprpaApproverStatus(MPRPAApproversModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var approverdata = obj.MPRPAApprovers.Where(x => x.PAId == model.PAId).FirstOrDefault();
                if (approverdata != null)
                {
                    approverdata.ApproversRemarks = model.ApproversRemarks;
                    approverdata.ApprovalStatus = model.ApprovalStatus;
                    approverdata.ApprovedOn = System.DateTime.Now;
                    obj.SaveChanges();
                    return status;
                }
                else
                {
                    return status;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string UpdateVendorCommunication(RfqCommunicationModel model)
        {
            string msg = "";
            try
            {
                if (model != null)
                {

                    int rfqccid = 0;
                    var remotedataforvendorcomm = new RemoteRFQCommunication();
                    remotedataforvendorcomm.Remarks = model.Remarks;
                    remotedataforvendorcomm.DeleteFlag = false;
                    remotedataforvendorcomm.RemarksDate = System.DateTime.Now;
                    remotedataforvendorcomm.RfqItemsId = model.RfqItemId;
                    remotedataforvendorcomm.RfqRevisionId = model.RfqRevisionId;
                    remotedataforvendorcomm.RemarksFrom = model.RemarksFrom;
                    vscm.RemoteRFQCommunications.Add(remotedataforvendorcomm);
                    vscm.SaveChanges();
                    rfqccid = remotedataforvendorcomm.RfqCCid;
                    var remotedataforvendorcommyscm = new RFQCommunication();
                    remotedataforvendorcommyscm.Remarks = model.Remarks;
                    remotedataforvendorcommyscm.DeleteFlag = false;
                    remotedataforvendorcommyscm.RemarksDate = System.DateTime.Now;
                    remotedataforvendorcommyscm.RfqItemsId = model.RfqItemId;
                    remotedataforvendorcommyscm.RfqRevisionId = model.RfqRevisionId;
                    remotedataforvendorcommyscm.RemarksFrom = model.RemarksFrom;
                    remotedataforvendorcommyscm.RfqCCid = rfqccid;
                    obj.RFQCommunications.Add(remotedataforvendorcommyscm);
                    obj.SaveChanges();

                    msg = "OK";


                }
            }
            catch (Exception e)
            {
                throw;
            }

            return msg;

            // throw new NotImplementedException();
        }

        public async Task<List<DisplayRfqTermsByRevisionId>> getrfqtermsbyrevisionid(List<int> RevisionId)
        {
            List<DisplayRfqTermsByRevisionId> revision = new List<DisplayRfqTermsByRevisionId>();
            try
            {
                revision = obj.DisplayRfqTermsByRevisionIds.Where(x => RevisionId.Contains(x.RFQrevisionId)).ToList();
                return revision;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }



}

