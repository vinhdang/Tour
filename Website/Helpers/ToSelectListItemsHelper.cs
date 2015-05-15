using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;

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
                              Selected = (role.ID == selectedId),
                              Text = role.Name,
                              Value = role.ID.ToString()
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
                              Selected = (c.ID == selectedId),
                              Text = c.Name,
                              Value = c.ID.ToString()
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
                              Selected = (c.ID == selectedId),
                              Text = c.Name,
                              Value = c.ID.ToString()
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
                              Selected = (c.ID == selectedId),
                              Text = c.Name,
                              Value = c.ID.ToString()
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
                              Selected = (c.ID == selectedId),
                              Text = c.Name,
                              Value = c.ID.ToString()
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
                              Selected = (c.ID == selectedId),
                              Text = c.Name,
                              Value = c.ID.ToString()
                          });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(
   this IEnumerable<TourTheme> TourTheme, int selectedId)
        {
            return

                TourTheme.OrderByDescending(c => c.Order)
                      .Select(c =>
                          new SelectListItem
                          {
                              Selected = (c.ID == selectedId),
                              Text = c.Name,
                              Value = c.ID.ToString()
                          });
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<TourType> TourType, int selectedId)
        {
            return

                TourType.OrderByDescending(c => c.Order)
                      .Select(c =>
                          new SelectListItem
                          {
                              Selected = (c.ID == selectedId),
                              Text = c.Name,
                              Value = c.ID.ToString()
                          });
        }
    

        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<Status> status, int selectedId)
        {
            return

                status.OrderByDescending(c => c.Order)
                      .Select(c =>
                          new SelectListItem
                          {
                              Selected = (c.ID == selectedId),
                              Text = c.Name,
                              Value = c.ID.ToString()
                          });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<TourProvider> tourProviders, int selectedId)
        {
            return tourProviders.OrderByDescending(c => c.ID)
                      .Select(c =>
                          new SelectListItem
                          {
                              Selected = (c.ID == selectedId),
                              Text = c.Name,
                              Value = c.ID.ToString()
                          });
        }

     
    }
}