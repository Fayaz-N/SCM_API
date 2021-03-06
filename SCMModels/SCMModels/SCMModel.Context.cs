﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SCMModels.SCMModels
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class YSCMEntities : DbContext
    {
        public YSCMEntities()
            : base("name=YSCMEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AccessGroupMaster> AccessGroupMasters { get; set; }
        public virtual DbSet<AccessName> AccessNames { get; set; }
        public virtual DbSet<AuthorizationItem> AuthorizationItems { get; set; }
        public virtual DbSet<AutorizationGroup> AutorizationGroups { get; set; }
        public virtual DbSet<ClientLogin> ClientLogins { get; set; }
        public virtual DbSet<ItemAttributesMaster> ItemAttributesMasters { get; set; }
        public virtual DbSet<MaterialMaster> MaterialMasters { get; set; }
        public virtual DbSet<MPRApprover> MPRApprovers { get; set; }
        public virtual DbSet<MPRBuyerGroupMember> MPRBuyerGroupMembers { get; set; }
        public virtual DbSet<MPRBuyerGroup> MPRBuyerGroups { get; set; }
        public virtual DbSet<MPRCommunication> MPRCommunications { get; set; }
        public virtual DbSet<MPRCustomsDuty> MPRCustomsDuties { get; set; }
        public virtual DbSet<MPRDepartment> MPRDepartments { get; set; }
        public virtual DbSet<MPRDespatchDocCategory> MPRDespatchDocCategories { get; set; }
        public virtual DbSet<MPRDetail> MPRDetails { get; set; }
        public virtual DbSet<MPRDispatchLocation> MPRDispatchLocations { get; set; }
        public virtual DbSet<MPRDocumentation> MPRDocumentations { get; set; }
        public virtual DbSet<MPRDocumentationDescription> MPRDocumentationDescriptions { get; set; }
        public virtual DbSet<MPRDocument> MPRDocuments { get; set; }
        public virtual DbSet<MPRIncharge> MPRIncharges { get; set; }
        public virtual DbSet<MPRIssuePurpos> MPRIssuePurposes { get; set; }
        public virtual DbSet<MPRItemDetails_X> MPRItemDetails_X { get; set; }
        public virtual DbSet<MPRItemDetailsAttachment> MPRItemDetailsAttachments { get; set; }
        public virtual DbSet<MPRItemInfo> MPRItemInfoes { get; set; }
        public virtual DbSet<MPROfferDetail> MPROfferDetails { get; set; }
        public virtual DbSet<MPRPAApprover> MPRPAApprovers { get; set; }
        public virtual DbSet<MPRPADetail> MPRPADetails { get; set; }
        public virtual DbSet<MPRPADocument> MPRPADocuments { get; set; }
        public virtual DbSet<MPRPAPurchaseMode> MPRPAPurchaseModes { get; set; }
        public virtual DbSet<MPRPAPurchaseType> MPRPAPurchaseTypes { get; set; }
        public virtual DbSet<MPRPAVendor> MPRPAVendors { get; set; }
        public virtual DbSet<MPRPreferredVendorType> MPRPreferredVendorTypes { get; set; }
        public virtual DbSet<MPRProcurementSource> MPRProcurementSources { get; set; }
        public virtual DbSet<MPRProjectDutyApplicable> MPRProjectDutyApplicables { get; set; }
        public virtual DbSet<MPRPurchaseType> MPRPurchaseTypes { get; set; }
        public virtual DbSet<MPRReminderTracking> MPRReminderTrackings { get; set; }
        public virtual DbSet<MPRRevision> MPRRevisions { get; set; }
        public virtual DbSet<MPRScope> MPRScopes { get; set; }
        public virtual DbSet<MPRStatu> MPRStatus { get; set; }
        public virtual DbSet<MPRStatusTrack> MPRStatusTracks { get; set; }
        public virtual DbSet<MPRTargetedSpendSupportingDoc> MPRTargetedSpendSupportingDocs { get; set; }
        public virtual DbSet<MPRVendorDetail> MPRVendorDetails { get; set; }
        public virtual DbSet<RFQCommunication> RFQCommunications { get; set; }
        public virtual DbSet<RFQDocument> RFQDocuments { get; set; }
        public virtual DbSet<RFQItem> RFQItems { get; set; }
        public virtual DbSet<RFQItemsInfo> RFQItemsInfoes { get; set; }
        public virtual DbSet<RFQMaster> RFQMasters { get; set; }
        public virtual DbSet<RFQReminderTracking> RFQReminderTrackings { get; set; }
        public virtual DbSet<RFQRevision> RFQRevisions { get; set; }
        public virtual DbSet<RFQStatu> RFQStatus { get; set; }
        public virtual DbSet<RFQVendorTerm> RFQVendorTerms { get; set; }
        public virtual DbSet<SCMStatu> SCMStatus { get; set; }
        public virtual DbSet<StandardListYNN> StandardListYNNs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TermsCategory> TermsCategories { get; set; }
        public virtual DbSet<Test_AttributeMaster> Test_AttributeMaster { get; set; }
        public virtual DbSet<Test_CategoryAttributes> Test_CategoryAttributes { get; set; }
        public virtual DbSet<Test_CategoryMaster> Test_CategoryMaster { get; set; }
        public virtual DbSet<Test_ItemAttributesMaster> Test_ItemAttributesMaster { get; set; }
        public virtual DbSet<Test_ItemMaster> Test_ItemMaster { get; set; }
        public virtual DbSet<testuserdetail> testuserdetails { get; set; }
        public virtual DbSet<UnitMaster> UnitMasters { get; set; }
        public virtual DbSet<UserDetail> UserDetails { get; set; }
        public virtual DbSet<VendorMaster> VendorMasters { get; set; }
        public virtual DbSet<VendorRFQTerm> VendorRFQTerms { get; set; }
        public virtual DbSet<CustomerMasterYG> CustomerMasterYGS { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<MPRTechClearance> MPRTechClearances { get; set; }
        public virtual DbSet<MPRApproversView> MPRApproversViews { get; set; }
    }
}
