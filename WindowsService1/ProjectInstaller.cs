using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace WMSDocumentSynchronizationService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            ServiceProcessInstaller spi = new ServiceProcessInstaller();
            spi.Account = ServiceAccount.LocalSystem;//设置服务要运行在什么类型的账号
            //这里可以创建多个ServiceInstaller实例
            ServiceInstaller si = new ServiceInstaller();
            si.ServiceName = "WMSDocumentSynchronizationService";//系统操作服务的标识，要和ServiceBase中设置的ServiceName属性值相同
            si.DisplayName = "WMSDocumentSynchronizationService";//展示给用户的服务名，即在控制面板中看到的服务名
            si.Description = "从ERP同步单据至WMS服务";
            si.StartType = ServiceStartMode.Automatic;//服务的启动方式，这里设置为自动

            //最后记得把创建的实例添加到安装列表中
            this.Installers.Add(si);
            this.Installers.Add(spi);
        }

        //注意必须重写Install和Uninstall方法，且在重写方法中必须调用基类对应的方法，否则在安装和卸载服务的过程中会出问题
        //小编就是因为没有调用基类中的方法导致安装和卸载出现问题
        //出此之外还有Commit、Rollback等方法
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
        }
    }
    }
