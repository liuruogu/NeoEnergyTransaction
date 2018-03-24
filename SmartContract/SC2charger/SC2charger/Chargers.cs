using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace sc3Consumer
{
    // this contract enable consumers rent car from Eve fleet manager (sc4) in available time slot
    // 
    public class Chargers : SmartContract
    {
        // token is the energy, 1 token = 1 kwh
        public static readonly byte[] Owner = "AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y".ToScriptHash();// log in user name
        // neo assetId: scriptHash
        public static byte[] neoAssetId = { 197, 111, 51, 252, 110, 207, 205, 12, 34, 92, 74, 179, 86, 254, 229, 147, 144, 175, 133, 96, 190, 14, 147, 15, 174, 190, 116, 166, 218, 255, 124, 155 };

        public delegate void MyAction<T, T1, T2, T3, T4, T5, T6>(T p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6);
        public static event MyAction<byte[], byte[], int, int, int, int, int> Transferred;
        public static Object Main(string operation, params object[] args)
        {
            if (Runtime.Trigger == TriggerType.Verification)
            {
                if (Owner.Length == 20)
                {
                    return Runtime.CheckWitness(Owner);
                }
                else if (Owner.Length == 33)
                {
                    byte[] signature = operation.AsByteArray();
                    return VerifySignature(signature, Owner);
                }
            }
            else if (Runtime.Trigger == TriggerType.Application)
            {
                if (operation == "rentChargers")
                {
                    //params: from, to, price, min, max, timeslot, chargerNum
                    if (args.Length != 5) return false;
                    byte[] from = (byte[])args[0];
                    byte[] to = (byte[])args[1];
                    int price = (int)args[2];
                    int minPowerRate = (int)args[3];
                    int maxPowerRate = (int)args[4];
                    int timeslot = (int)args[5];
                    int chargerNum = (int)args[6];
                    return rentChargers(from, to, price, minPowerRate, maxPowerRate, timeslot, chargerNum);
                }
                if (operation == "buyEnergy")
                {
                    // params: from, volume
                    if (args.Length != 1) return false;
                    byte[] from = (byte[])args[0];
                    return CheckChargerNum(from);
                }
                if (operation == "balanceOf")
                {
                    if (args.Length != 1) return false;
                    byte[] from = (byte[])args[0];
                    return BalanceOf(from);
                }
            }
            return false;
        }
        public static bool rentChargers(byte[] from, byte[] to, int price, int minPowerRate, int maxPowerRate, int timeslot, int chargerNum)
        {
            if (price < 0) return false;
            if (minPowerRate < 0 || maxPowerRate < 0 || maxPowerRate < minPowerRate) return false;
            if (timeslot < 0 || timeslot > 7) return false;
            if (!CheckAvailableTime(timeslot)) return false;
            if (!Runtime.CheckWitness(from)) return false;
            if (from == to) return true;
            int amount = price * 2 * chargerNum;

            // subtract charger num from sc2 users (chargers)
            BigInteger from_charger_num = CheckChargerNum(from);
            if (chargerNum > from_charger_num) return false;
            if (chargerNum == from_charger_num)
                Storage.Delete(Storage.CurrentContext, from);
            else
                Storage.Put(Storage.CurrentContext, from, from_charger_num - chargerNum);

            // add balance to sc2 users
            Update_balance(from, amount);

            // substract balance of buyer
            Update_balance(to, -amount);

            Transferred(from, to, price, minPowerRate, maxPowerRate, timeslot, chargerNum);
            return true;
        }

        public static bool CheckAvailableTime(int timeslot)
        {
            // 1 timeslot equals 3 hours, 1 day has 8 timeslots
            if (timeslot == 1 || timeslot == 5 || timeslot == 7) return true;
            else return false;
            //int[] currentAvailableTime = new int[4] { 3, 5, 7, 1 };
            //int id = Array.IndexOf(currentAvailableTime, timeslot);
            //if (id == -1) return false;
            //else return true;
        }

        public static bool BuyEnergy(byte[] from, byte[] to, BigInteger price, BigInteger volume)
        {
            // tbd
            return true;
        }

        public static BigInteger CheckChargerNum(byte[] addr)
        {
            return Storage.Get(Storage.CurrentContext, addr).AsBigInteger();
        }

        public static BigInteger BalanceOf(byte[] from)
        {
            Account account = Blockchain.GetAccount(from);
            long balance = account.GetBalance(neoAssetId);
            return balance;
        }

        public static BigInteger Update_balance(byte[] addr, BigInteger amount)
        {
            return BalanceOf(addr) + amount * 100000000;
        }
    }
}
