using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework.Services.System;
using System;
using System.Numerics;
using System.ComponentModel;

namespace SC1prosumer
{
    public class Prosumer : SmartContract
    {
        // token is the energy, 1 token = 1 kwh
        public static readonly byte[] Owner = "AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y".ToScriptHash();
        // neo assetId: scriptHash
        public static byte[] neoAssetId = { 197, 111, 51, 252, 110, 207, 205, 12, 34, 92, 74, 179, 86, 254, 229, 147, 144, 175, 133, 96, 190, 14, 147, 15, 174, 190, 116, 166, 218, 255, 124, 155 };

        public delegate void MyAction<T, T1, T2, T3>(T p0, T1 p1, T2 p2, T3 p3);
        public static event MyAction<byte[], byte[], int, int> Transferred;
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
                if (operation == "sellSurplus")
                {
                    // params: from, to, price, surplus volume
                    if (args.Length != 4) return false;
                    byte[] from = (byte[])args[0];
                    byte[] to = (byte[])args[1];
                    int price = (int)args[2];
                    int volume = (int)args[3];
                    return SellSurplus(from, to, price, volume);
                }
                if (operation == "txHistory")
                {
                    // tbd
                }
                if (operation == "checksurplus")
                {
                    // params: from
                    if (args.Length != 1) return false;
                    byte[] from = (byte[])args[0];
                    return CheckSurplus(from);
                }
                if (operation == "balanceOf")
                {
                    if (args.Length != 1) return false;
                    byte[] from = (byte[])args[0];
                    return BalanceOf(from);
                }
            }
            return false;
            // make sure this is the client
            //if (!VerifySignature(signature, client)) return false;

            // get account of agent ad client
            //Account ageAccount = Blockchain.GetAccount(agent);

            // get balance (surplus) of agent's account
            //long balance = ageAccount.GetBalance(assetId);

            // see if there's enough surplus
            //if (balance > volume * 100000000) return true;
            //else return false;
            

            // create contract -- tbd
        }

        public static bool SellSurplus(byte[] from, byte[] to, int price, int volume)
        {
            if (price < 0) return false;
            if (volume < 0) return false;
            if (!Runtime.CheckWitness(from)) return false;
            if (from == to) return true;
            int amount = price * volume;

            // substract surplus volume from sc1 user(seller)
            BigInteger from_volume = CheckSurplus(from);
            if (volume > from_volume) return false;
            if (from_volume == volume)
                Storage.Delete(Storage.CurrentContext, from);
            else
                Storage.Put(Storage.CurrentContext, from, from_volume - volume);
            // add balance to seller
            BigInteger from_balance = Update_balance(from, amount);

            // add surplus volume to buyer
            BigInteger to_volume = Storage.Get(Storage.CurrentContext, to).AsBigInteger();
            Storage.Put(Storage.CurrentContext, to, to_volume + volume);

            // substract balance of buyer
            BigInteger to_balance = Update_balance(to, -amount);
            Transferred(from, to, price, volume);
            return true;
        }

        public static BigInteger CheckSurplus(byte[] from)
        {
            return Storage.Get(Storage.CurrentContext, from).AsBigInteger();
        }

        public static BigInteger BalanceOf (byte[] from)
        {
            Account account = Blockchain.GetAccount(from);
            long balance = account.GetBalance(neoAssetId);
            return balance;
        }

        public static BigInteger Update_balance (byte[] addr, int amount)
        {
            BigInteger updated_balance = BalanceOf(addr) + amount;
            return updated_balance;
        }
    }
}
