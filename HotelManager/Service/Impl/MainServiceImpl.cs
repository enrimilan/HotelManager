using HotelManager.Dao;
using System;

namespace HotelManager.Service.Impl
{
    public class MainServiceImpl : MainService
    {

        private MainDao mainDao;

        public MainServiceImpl(MainDao mainDao)
        {
            this.mainDao = mainDao;
        }

        public void Close()
        {
            mainDao.Close();
        }

    }
}
