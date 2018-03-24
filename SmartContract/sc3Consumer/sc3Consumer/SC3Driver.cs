using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace sc3Consumer
{
    // this contract enable consumers rent car from Eve fleet manager (sc4) in available time slot
    // 
    public class SC3Driver : SmartContract
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
                if (operation == "chargeCar")
                {
                    //params: from, to, price, min, max, timeslot, chargerNum
                    if (args.Length != 4) return false;
                    byte[] from = (byte[])args[0];
                    int price = (int)args[1];
                    int timeslot = (int)args[2];
                    int volume = (int)args[3];
                    return ChargeCar(from, price, timeslot, volume);
                }
                if (operation == "rentCar")
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
        public static bool ChargeCar(byte[] from, int price, int timeslot, int volume)
        {
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
