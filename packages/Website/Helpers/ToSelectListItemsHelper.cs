using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entities;

namespace Website.Helpers
{
    public static class ToSelectListItemsHelper
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(
           this IEnumerable<Role> roles, int selectedId)
        {
            return

                roles.OrderByDescending(role => role.Order)
                      .Select(role =>
                          new SelectListItem
                          {
                              Selected = (role.RoleID == selectedId),
                              Text = role.Name,
                              Value = role.RoleID.ToString()
                          });
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(
          this IEnumerable<Category> categories, int selectedId)
        {
            return

                categories.OrderByDescending(c => c.Order)
                      .Select(c =>
                          new SelectListItem
                          {
                              Selected = (c.CategoryID == selectedId),
                              Text = c.Name,
                              Value = c.CategoryID.ToString()
                          });
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(
this IEnumerable<AdminMenu> adminMenu, int selectedId)
        {
            return

                adminMenu.OrderByDescending(c => c.Order)
                      .Select(c =>
                          new SelectListItem
                          {
                              Selected = (c.MenuID == selectedId),
                              Text = c.Name,
                              Value = c.MenuID.ToString()
                          });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(
this IEnumerable<National> national, int selectedId)
        {
            return

                national.OrderByDescending(c => c.Order)
                      .Select(c =>
                          new SelectListItem
                          {
                              Selected = (c.NationalID == selectedId),
                              Text = c.Name,
                              Value = c.NationalID.ToString()
                          });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(
this IEnumerable<Province> province, int selectedId)
        {
            return

                province.OrderByDescending(c => c.Order)
                      .Select(c =>
                          new SelectListItem
                          {
                              Selected = (c.ProvinceID == selectedId),
                              Text = c.Name,
                              Value = c.ProvinceID.ToString()
                          });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(
this IEnumerable<District> district, int selectedId)
        {
            return

                district.OrderByDescending(c => c.Order)
                      .Select(c =>
                          new SelectListItem
                          {
                              Selected = (c.DistrictID == selectedId),
                              Text = c.Name,
                              Value = c.DistrictID.ToString()
                          });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(
this IEnumerable<Area> area, int selectedId)
        {
            return

                area.OrderByDescending(c => c.Order)
                      .Select(c =>
                          new SelectListItem
                          {
                              Selected = (c.AreaID == selectedId),
                              Text = c.Name,
                              Value = c.AreaID.ToString()
                          });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(
this IEnumerable<HotelType> hotelType, int selectedId)
        {
            return

                hotelType.OrderByDescending(c => c.Order)
                      .Select(c =>
                          new SelectListItem
                          {
                              Selected = (c.HotelTypeID == selectedId),
                              Text = c.Name,
                              Value = c.HotelTypeID.ToString()
                          });
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(
this IEnumerable<HotelTheme> hotelTheme, int selectedId)
        {
            return

                hotelTheme.OrderByDescending(c => c.Order)
                      .Select(c =>
                          new SelectListItem
                          {
                              Selected = (c.HotelThemeID == selectedId),
                              Text = c.Name,
                              Value = c.HotelThemeID.ToString()
                          });
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(
this IEnumerable<Room> room, int selectedId)
        {
            return

                room.OrderByDescending(c => c.Order)
                      .Select(c =>
                          new SelectListItem
                          {
                              Selected = (c.RoomID == selectedId),
                              Text = c.Name,
                              Value = c.RoomID.ToString()
                          });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(
this IEnumerable<Status> status, int selectedId)
        {
            return

                status.OrderByDescending(c => c.Order)
                      .Select(c =>
                          new SelectListItem
                          {
                              Selected = (c.StatusID == selectedId),
                              Text = c.Name,
                              Value = c.StatusID.ToString()
                          });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(
this IEnumerable<CompareType> compareTypes, int selectedId)
        {
            return

                compareTypes.OrderByDescending(c => c.Order)
                      .Select(c =>
                          new SelectListItem
                          {
                              Selected = (c.CompareTypeID == selectedId),
                              Text = c.Name,
                              Value = c.CompareTypeID.ToString()
                          });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(
this IEnumerable<Hotel> hotels, int selectedId)
        {
            return

                hotels.OrderByDescending(c => c.Order)
                      .Select(c =>
                          new SelectListItem
                          {
                              Selected = (c.HotelID == selectedId),
                              Text = c.Name,
                              Value = c.HotelID.ToString()
                          });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(
this IEnumerable<GroupTemplate> group, int selectedId)
        {
            return

                group.OrderByDescending(c => c.GroupTemplateID).Select(c =>
                          new SelectListItem
                          {
                              Selected = (c.GroupTemplateID == selectedId),
                              Text = c.Name,
                              Value = c.GroupTemplateID.ToString()
                          });
        }


    }
}