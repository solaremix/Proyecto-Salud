﻿using Interface;
using System;

namespace Service.Svc
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService
    {
        public int GetRandomNumber()
        {
            Random random = new Random();
            return random.Next(1, 100);  // Devuelve un número aleatorio entre 1 y 99.
        }
    }
}
