using System.Web.Mvc;
using Microsoft.Practices.Unity;
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


            container.RegisterType<INewRepository, NewRepository>(new HttpContextLifetimeManager<INewRepository>());
            container.RegisterType<INewService, NewService>(new HttpContextLifetimeManager<INewService>());

            container.RegisterType<IConfigRepository, ConfigRepository>(new HttpContextLifetimeManager<IConfigRepository>());
            container.RegisterType<IConfigService, ConfigService>(new HttpContextLifetimeManager<IConfigService>());


            container.RegisterType<IOrderRepository, OrderRepository>(new HttpContextLifetimeManager<IOrderRepository>());
            container.RegisterType<IOrderService, OrderService>(new HttpContextLifetimeManager<IOrderService>());

            container.RegisterType<IAdminMenuRepository, AdminMenuRepository>(new HttpContextLifetimeManager<IAdminMenuRepository>());
            container.RegisterType<IAdminMenuService, AdminMenuService>(new HttpContextLifetimeManager<IAdminMenuService>());



            container.RegisterType<IContactRepository, ContactRepository>(new HttpContextLifetimeManager<IContactRepository>());
            container.RegisterType<IContactService, ContactService>(new HttpContextLifetimeManager<IContactService>());



            container.RegisterType<IBannerRepository, BannerRepository>(new HttpContextLifetimeManager<IBannerRepository>());
            container.RegisterType<IBannerService, BannerService>(new HttpContextLifetimeManager<IBannerService>());


            container.RegisterType<ISupportRepository, SupportRepository>(new HttpContextLifetimeManager<ISupportRepository>());
            container.RegisterType<ISupportService, SupportService>(new HttpContextLifetimeManager<ISupportService>());

            container.RegisterType<ICategoryPictureRepository, CategoryPictureRepository>(new HttpContextLifetimeManager<ICategoryPictureRepository>());
            container.RegisterType<ICategoryPictureService, CategoryPictureService>(new HttpContextLifetimeManager<ICategoryPictureService>());

            container.RegisterType<INationalRepository, NationalRepository>(new HttpContextLifetimeManager<INationalRepository>());
            container.RegisterType<INationalService, NationalService>(new HttpContextLifetimeManager<INationalService>());

            container.RegisterType<IProvinceRepository, ProvinceRepository>(new HttpContextLifetimeManager<IProvinceRepository>());
            container.RegisterType<IProvinceService, ProvinceService>(new HttpContextLifetimeManager<IProvinceService>());

            container.RegisterType<IDistrictRepository, DistrictRepository>(new HttpContextLifetimeManager<IDistrictRepository>());
            container.RegisterType<IDistrictService, DistrictService>(new HttpContextLifetimeManager<IDistrictService>());

            container.RegisterType<IAreaRepository, AreaRepository>(new HttpContextLifetimeManager<IAreaRepository>());
            container.RegisterType<IAreaService, AreaService>(new HttpContextLifetimeManager<IAreaService>());

            container.RegisterType<IComfortRepository, ComfortRepository>(new HttpContextLifetimeManager<IComfortRepository>());
            container.RegisterType<IComfortService, ComfortService>(new HttpContextLifetimeManager<IComfortService>());

            container.RegisterType<IHotelThemeRepository, HotelThemeRepository>(new HttpContextLifetimeManager<IHotelThemeRepository>());
            container.RegisterType<IHotelThemeService, HotelThemeService>(new HttpContextLifetimeManager<IHotelThemeService>());

            container.RegisterType<IHotelTypeRepository, HotelTypeRepository>(new HttpContextLifetimeManager<IHotelTypeRepository>());
            container.RegisterType<IHotelTypeService, HotelTypeService>(new HttpContextLifetimeManager<IHotelTypeService>());

            container.RegisterType<IPaymentRepository, PaymentRepository>(new HttpContextLifetimeManager<IPaymentRepository>());
            container.RegisterType<IPaymentService, PaymentService>(new HttpContextLifetimeManager<IPaymentService>());

            container.RegisterType<IHotelRepository, HotelRepository>(new HttpContextLifetimeManager<IHotelRepository>());
            container.RegisterType<IHotelService, HotelService>(new HttpContextLifetimeManager<IHotelService>());

            container.RegisterType<IHotelPictureRepository, HotelPictureRepository>(new HttpContextLifetimeManager<IHotelPictureRepository>());
            container.RegisterType<IHotelPictureService, HotelPictureService>(new HttpContextLifetimeManager<IHotelPictureService>());

            container.RegisterType<IRoomTypeRepository, RoomTypeRepository>(new HttpContextLifetimeManager<IRoomTypeRepository>());
            container.RegisterType<IRoomTypeService, RoomTypeService>(new HttpContextLifetimeManager<IRoomTypeService>());

            container.RegisterType<IRoomRepository, RoomRepository>(new HttpContextLifetimeManager<IRoomRepository>());
            container.RegisterType<IRoomService, RoomService>(new HttpContextLifetimeManager<IRoomService>());

            container.RegisterType<IRoomPriceRepository, RoomPriceRepository>(new HttpContextLifetimeManager<IRoomPriceRepository>());
            container.RegisterType<IRoomPriceService, RoomPriceService>(new HttpContextLifetimeManager<IRoomPriceService>());

            container.RegisterType<ICompareRepository, CompareRepository>(new HttpContextLifetimeManager<ICompareRepository>());
            container.RegisterType<ICompareService, CompareService>(new HttpContextLifetimeManager<ICompareService>());

            container.RegisterType<IOrderInfoRepository, OrderInfoRepository>(new HttpContextLifetimeManager<IOrderInfoRepository>());
            container.RegisterType<IOrderInfoService, OrderInfoService>(new HttpContextLifetimeManager<IOrderInfoService>());
            container.RegisterType<IOrderRepository, OrderRepository>(new HttpContextLifetimeManager<IOrderRepository>());
            container.RegisterType<IOrderService, OrderService>(new HttpContextLifetimeManager<IOrderService>());

            container.RegisterType<IStatusRepository, StatusRepository>(new HttpContextLifetimeManager<IStatusRepository>());
            container.RegisterType<IStatusService, StatusService>(new HttpContextLifetimeManager<IStatusService>());


            container.RegisterType<IAirlineRepository, AirlineRepository>(new HttpContextLifetimeManager<IAirlineRepository>());
            container.RegisterType<IAirlineService, AirlineService>(new HttpContextLifetimeManager<IAirlineService>());

            container.RegisterType<ISeatTypeRepository, SeatTypeRepository>(new HttpContextLifetimeManager<ISeatTypeRepository>());
            container.RegisterType<ISeatTypeService, SeatTypeService>(new HttpContextLifetimeManager<ISeatTypeService>());

            container.RegisterType<ITicketRepository, TicketRepository>(new HttpContextLifetimeManager<ITicketRepository>());
            container.RegisterType<ITicketService, TicketService>(new HttpContextLifetimeManager<ITicketService>());

            container.RegisterType<ITicketTypeRepository, TicketTypeRepository>(new HttpContextLifetimeManager<ITicketRepository>());
            container.RegisterType<ITicketService, TicketService>(new HttpContextLifetimeManager<ITicketService>());


            container.RegisterType<ICompareTypeRepository, CompareTypeRepository>(new HttpContextLifetimeManager<ICompareTypeRepository>());
            container.RegisterType<ICompareTypeService, CompareTypeService>(new HttpContextLifetimeManager<ICompareTypeService>());

            container.RegisterType<IEmailRepository, EmailRepository>(new HttpContextLifetimeManager<IEmailRepository>());
            container.RegisterType<IEmailService, EmailService>(new HttpContextLifetimeManager<IEmailService>());

            container.RegisterType<IEmailTemplateRepository, EmailTemplateRepository>(new HttpContextLifetimeManager<IEmailTemplateRepository>());
            container.RegisterType<IEmailTemplateService, EmailTemplateService>(new HttpContextLifetimeManager<IEmailTemplateService>());

            container.RegisterType<ISeatTypeRepository, SeatTypeRepository>(new HttpContextLifetimeManager<ISeatTypeRepository>());
            container.RegisterType<ISeatTypeService, SeatTypeService>(new HttpContextLifetimeManager<ISeatTypeService>());

            container.RegisterType<ITicketTypeRepository, TicketTypeRepository>(new HttpContextLifetimeManager<ITicketTypeRepository>());
            container.RegisterType<ITicketTypeService, TicketTypeService>(new HttpContextLifetimeManager<ITicketTypeService>());


            container.RegisterType<IEmailSettingRepository, EmailSettingRepository>(new HttpContextLifetimeManager<IEmailSettingRepository>());
            container.RegisterType<IEmailSettingService, EmailSettingService>(new HttpContextLifetimeManager<IEmailSettingService>());


            container.RegisterType<IGroupTemplateRepository, GroupTemplateRepository>(new HttpContextLifetimeManager<IGroupTemplateRepository>());
            container.RegisterType<IGroupTemplateService, GroupTemplateService>(new HttpContextLifetimeManager<IGroupTemplateService>());


            container.RegisterType<IEmailGetPriceRepository, EmailGetPriceRepository>(new HttpContextLifetimeManager<IEmailGetPriceRepository>());
            container.RegisterType<IEmailGetPriceService, EmailGetPriceService>(new HttpContextLifetimeManager<IEmailGetPriceService>());

            container.RegisterType<ITicketRepository, TicketRepository>(new HttpContextLifetimeManager<ITicketRepository>());
            container.RegisterType<ITicketService, TicketService>(new HttpContextLifetimeManager<ITicketService>());

            container.RegisterType<ICommentRepository, CommentRepository>(new HttpContextLifetimeManager<ICommentRepository>());
            container.RegisterType<ICommentService, CommentService>(new HttpContextLifetimeManager<ICommentService>());

            return container;
        }
    }
}