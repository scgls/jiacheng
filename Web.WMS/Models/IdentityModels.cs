using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Web.WMS.Models
{
    // 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<BILWeb.Area.T_AreaInfo> T_AreaInfo { get; set; }

        public System.Data.Entity.DbSet<BILWeb.Warehouse.T_WareHouseInfo> T_WareHouseInfo { get; set; }

        public System.Data.Entity.DbSet<BILWeb.House.T_HouseInfo> T_HouseInfo { get; set; }

        public System.Data.Entity.DbSet<BILWeb.Material.T_MaterialInfo> T_MaterialInfo { get; set; }

        public System.Data.Entity.DbSet<BILWeb.Query.Check_Model> Check_Model { get; set; }

        public System.Data.Entity.DbSet<BILWeb.PickRule.T_PickRuleInfo> T_PickRuleInfo { get; set; }

        public System.Data.Entity.DbSet<BILWeb.DepInterface.T_DepInterfaceInfo> T_DepInterfaceInfo { get; set; }

        public System.Data.Entity.DbSet<BILWeb.UserGroup.T_UserGroupInfo> T_UserGroupInfo { get; set; }

        public System.Data.Entity.DbSet<BILWeb.Supplier.T_SupplierInfo> T_SupplierInfo { get; set; }

        public System.Data.Entity.DbSet<BILWeb.Customer.T_CustomerInfo> T_CustomerInfo { get; set; }

        public System.Data.Entity.DbSet<BILWeb.OutStock.T_OutStockInfo> T_OutStockInfo { get; set; }
    }
}