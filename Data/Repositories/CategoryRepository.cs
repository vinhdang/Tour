using System.Collections.Generic;
using System.Linq;
using Data.Infrastructure;
using Domain;
using Data.IRepositories;

namespace Data.Repositories
{
    public class CategoryRepository : EntityRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

        #region Admin
        public List<Category> GetAll()
        {
            List<Category> list = (from cat in this.DataContext.Categories
                                   where cat.ParentID == -1
                                   orderby cat.Order descending, cat.CreatedDate descending

                                   select cat

                                  ).ToList();
            List<Category> _listresult = new List<Category>();
            foreach (Category item in list)
            {
                if (!_listresult.Contains(item))
                {
                    _listresult.Add(item);
                    getCategories(item.ID, ref list, _listresult);
                }
            }
            return _listresult;

        }

        #endregion

        #region Private
        private void getCategories(int catID, ref List<Category> list, List<Category> listresult)
        {
            List<Category> temp = (from cat in this.DataContext.Categories
                                   where cat.ParentID == catID 
                                   orderby cat.Order descending, cat.CreatedDate descending
                                   select cat
                                       ).ToList();
            foreach (Category item in temp)
            {
                if (!listresult.Contains(item))
                {
                    string str = "";
                    if (item.Level == 1)
                    {
                        str = " - - - ";
                    }
                    if (item.Level == 2)
                    {
                        str = " - - - - - - - - ";
                    }
                    if (item.Level == 3)
                    {
                        str = " - - - - - - - - - - - ";
                    }
                    if (item.Level == 4)
                    {
                        str = " - - - - - - - - - - - - - - ";
                    }
                    if (item.Level == 5)
                    {
                        str = " - - - - - - - ";
                    }
                    if (item.Level == 6)
                    {
                        str = " - - - - - - - - ";
                    }
                    if (item.Level == 7)
                    {
                        str = " - - - - - - - - - ";
                    }
                    if (item.Level == 8)
                    {
                        str = " - - - - - - - - - ";
                    }
                    if (item.Level == 9)
                    {
                        str = " - - - - - - - - - - ";
                    }
                    item.Name = str + item.Name.Replace("-", "");
                    listresult.Add(item);
                    getCategories(item.ID, ref list, listresult);
                }
            }
        }
        #endregion

    }
}
