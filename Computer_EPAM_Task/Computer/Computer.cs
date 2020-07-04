﻿using Computer_EPAM_Task.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Computer_EPAM_Task.Computer
{
    internal class Computer : IComputer
    {
        private ManagementObjectSearcher searcher;

        public Computer() { }

        public (string, string) GetInfoAboutCPU()
        {
            using (searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Name, NumberOfCores FROM Win32_Processor"))
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    return (item["Name"].ToString(), item["NumberOfCores"].ToString());
                }
            }

            throw new Exception("При считывании данных о центральном процессоре произошла ошибка");
        }

        public (string, string, string, string) GetInfoAboutGPU()
        {
            byte tmp = 0;
            string tmp1 = null;
            string tmp2 = null;

            using (searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Caption, VideoProcessor FROM Win32_VideoController"))
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    switch (tmp++)
                    {
                        case 0:
                            tmp1 = item["Caption"].ToString();
                            tmp2 = item["VideoProcessor"].ToString();
                            break;
                        default:
                            return (tmp1, tmp2, item["Caption"].ToString(), item["VideoProcessor"].ToString());
                    }
                }

                throw new Exception("При считывании данных о видеокарте произошла ошибка");
            }
        }

        public (string, string) GetInfoAboutOS()
        {
            using (searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Caption, Version FROM Win32_OperatingSystem"))
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    return (item["Caption"].ToString(), item["Version"].ToString());
                }
            }

            throw new Exception("При считывании данных о операционной системе произошла ошибка");
        }

        public (string, string) GetInfoAboutRAM()
        {
            double capacity = 0;
            double freq = 0;

            using (searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Capacity, Speed FROM Win32_PhysicalMemory"))
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    capacity += Math.Round(Convert.ToDouble(item.Properties["Capacity"].Value) / 1024 / 1024 / 1024, 2);
                    if (freq < Convert.ToDouble(item["Speed"].ToString().Substring(0, 4)))
                    {
                        freq = Convert.ToDouble(item["Speed"].ToString().Substring(0, 4));
                    }
                }

                return (capacity.ToString(), freq.ToString());
            }

            throw new Exception("При считывании данных о оперативной памяти произошла ошибка");
        }
    }
}