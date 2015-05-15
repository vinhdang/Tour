using System.Data.Entity;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Service.Implementations;
using Unity.Mvc3;
using Data.Infrastructure;
using Website.IoC;
using Data.Repositories;
using Data.IRepositories;
using Service.IServices;
using Service;
using Website.Log;
namespace Website
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();
            
            DependencyResolver.SetResolver(new Website.IoC.UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HttpContextLifetimeManager<IUnitOfWork>());
            container.RegisterType<IDatabaseFactory, DatabaseFactory>(new HttpContextLifetimeManager<IDatabaseFactory>());

            container.RegisterType<IRoleRepository, RoleRepository>(new HttpContextLifetimeManager<IRoleRepository>());
            container.RegisterType<IRoleService, RoleService>(new HttpContextLifetimeManager<IRoleService>());

            container.RegisterType<IUserRepository, UserRepository>(new HttpContextLifetimeManager<IUserRepository>());
            container.RegisterType<IUserService, UserService>(new HttpContextLifetimeManager<IUserService>());

            container.RegisterType<ICategoryRepository, CategoryRepository>(new HttpContextLifetimeManager<ICategoryRepository>());
            container.RegisterType<ICategoryService, CategoryService>(new HttpContextLifetimeManager<ICategoryService>());


            container.RegisterType<IRolePermissionRepository, RolePermissionRepository>(new HttpContextLifetimeManager<IRolePermissionRepository>());
            container.RegisterType<IRolePermissionService, RolePermissionService>(new HttpContextLifetimeManager<IRolePermissionService>());

            container.RegisterType<IConfigRepository, ConfigRepository>(new HttpContextLifetimeManager<IConfigRepository>());
            container.RegisterType<IConfigService, ConfigService>(new HttpContextLifetimeManager<IConfigService>());
            container.RegisterType<ISocialRepository, SocialRepository>(new HttpContextLifetimeManager<ISocialRepository>());
            container.RegisterType<ISocialService, SocialService>(new HttpContextLifetimeManager<ISocialService>());
            container.RegisterType<INationalRepository, NationalRepository>(new HttpContextLifetimeManager<INationalRepository>());
            container.RegisterType<INationalService, NationalService>(new HttpContextLifetimeManager<INationalService>());

            container.RegisterType<IProvinceRepository, ProvinceRepository>(new HttpContextLifetimeManager<IProvinceRepository>());
            container.RegisterType<IProvinceService, ProvinceService>(new HttpContextLifetimeManager<IProvinceService>());

            container.RegisterType<IDistrictRepository, DistrictRepository>(new HttpContextLifetimeManager<IDistrictRepository>());
            container.RegisterType<IDistrictService, DistrictService>(new HttpContextLifetimeManager<IDistrictService>());

            container.RegisterType<IAreaRepository, AreaRepository>(new HttpContextLifetimeManager<IAreaRepository>());
            container.RegisterType<IAreaService, AreaService>(new HttpContextLifetimeManager<IAreaService>());

            container.RegisterType<IProviderPictureRepository, ProviderPictureRepository>(new HttpContextLifetimeManager<IProviderPictureRepository>());
            container.RegisterType<IProviderPictureService, ProviderPictureService>(new HttpContextLifetimeManager<IProviderPictureService>());

            container.RegisterType<ITourThemeRepository, TourThemeRepository>(new HttpContextLifetimeManager<ITourThemeRepository>());
            container.RegisterType<ITourThemeService, TourThemeService>(new HttpContextLifetimeManager<ITourThemeService>());

            container.RegisterType<IPaymentRepository, PaymentRepository>(new HttpContextLifetimeManager<IPaymentRepository>());
            container.RegisterType<IPaymentService, PaymentService>(new HttpContextLifetimeManager<IPaymentService>());

            container.RegisterType<ITourCommentRepository, TourCommentRepository>(new HttpContextLifetimeManager<ITourCommentRepository>());
            container.RegisterType<ITourCommentService, TourCommentService>(new HttpContextLifetimeManager<ITourCommentService>());

            container.RegisterType<ISupportRepository, SupportRepository>(new HttpContextLifetimeManager<ISupportRepository>());
            container.RegisterType<ISupportService, SupportService>(new HttpContextLifetimeManager<ISupportService>());

            container.RegisterType<IContactRepository, ContactRepository>(new HttpContextLifetimeManager<IContactRepository>());
            container.RegisterType<IContactService, ContactService>(new HttpContextLifetimeManager<IContactService>());

            container.RegisterType<IStatusRepository, StatusRepository>(new HttpContextLifetimeManager<IStatusRepository>());
            container.RegisterType<IStatusService, StatusService>(new HttpContextLifetimeManager<IStatusService>());

            container.RegisterType<ITourTypeRepository, TourTypeRepository>(new HttpContextLifetimeManager<ITourTypeRepository>());
            container.RegisterType<ITourTypeService, TourTypeService>(new HttpContextLifetimeManager<ITourTypeService>());

            container.RegisterType<ITourProviderRepository, TourProviderRepository>(new HttpContextLifetimeManager<ITourProviderRepository>());
            container.RegisterType<ITourProviderService, TourProviderService>(new HttpContextLifetimeManager<ITourProviderService>());

            container.RegisterType<ITourAgendaRepository, TourAgendaRepository>(new HttpContextLifetimeManager<ITourAgendaRepository>());
            container.RegisterType<ITourAgendaService, TourAgendaService>(new HttpContextLifetimeManager<ITourAgendaService>());

            container.RegisterType<ITourActivityRepository, TourActivityRepository>(new HttpContextLifetimeManager<ITourActivityRepository>());
            container.RegisterType<ITourActivityService, TourActivityService>(new HttpContextLifetimeManager<ITourAgendaService>());

            container.RegisterType<ITourRepository, TourRepository>(new HttpContextLifetimeManager<ITourRepository>());
            container.RegisterType<ITourService, TourService>(new HttpContextLifetimeManager<ITourService>());

            container.RegisterType<ITourServiceRepository, TourServiceRepository>(new HttpContextLifetimeManager<ITourServiceRepository>());
            container.RegisterType<ITourUtilitiesService, TourUtilitiesService>(new HttpContextLifetimeManager<ITourUtilitiesService>());

            container.RegisterType<ITourPictureRepository, TourPictureRepository>(new HttpContextLifetimeManager<ITourPictureRepository>());
            container.RegisterType<ITourPictureService, TourPictureService>(new HttpContextLifetimeManager<ITourPictureService>());

            container.RegisterType<ITourPriceRepository, TourPriceRepository>(new HttpContextLifetimeManager<ITourPriceRepository>());
            container.RegisterType<ITourPriceService, TourPriceService>(new HttpContextLifetimeManager<ITourPriceService>());
            return container;
        }
    }
}