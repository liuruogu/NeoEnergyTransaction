//using neo.smartcontract.framework;
//using neo.smartcontract.framework.services.neo;
//using neo.smartcontract.framework.services.system;
//using system;
//using system.componentmodel;
//using system.numerics;

//namespace neo.smartcontract
//{
//    public class openpoint : framework.smartcontract
//    {
//        //token settings
//        public static string name() => "open point token";
//        public static string symbol() => "opt";
//        public static readonly byte[] owner = "ak2njjpjr6o664cwjki1qrxjqeic2zrp8y".toscripthash();
//        public static byte decimals() => 0;
//        private const ulong factor = 100000000; //decided by decimals()
//        private const ulong neo_decimals = 100000000;

//        //for the simplicity sake, we don't allow sending neo and get back tokens.
//        //in the future, the contract owner can set the exchange rate and we could allow that.

//        public delegate void myaction<t, t1>(t p0, t1 p1);
//        public delegate void myaction<t, t1, t2>(t p0, t1 p1, t2 p2);

//        [displayname("transfer")]
//        public static event myaction<byte[], byte[], biginteger> transferred;

//        public static object main(string operation, params object[] args)
//        {

//            if (runtime.trigger == triggertype.verification)
//            {
//                if (owner.length == 20)
//                {
//                    // if param owner is script hash
//                    return runtime.checkwitness(owner);
//                }
//                else if (owner.length == 33)
//                {
//                    // if param owner is public key
//                    byte[] signature = operation.asbytearray();
//                    return verifysignature(signature, owner);
//                }
//            }
//            else if (runtime.trigger == triggertype.application)
//            {
//                if (operation == "totalsupply") return totalsupply();
//                if (operation == "name") return name();
//                if (operation == "symbol") return symbol();
//                if (operation == "transfer")
//                {
//                    if (args.length != 3) return false;
//                    byte[] from = (byte[])args[0];
//                    byte[] to = (byte[])args[1];
//                    biginteger value = (biginteger)args[2];
//                    return transfer(from, to, value);
//                }
//                if (operation == "balanceof")
//                {
//                    if (args.length != 1) return 0;
//                    byte[] account = (byte[])args[0];
//                    return balanceof(account);
//                }

//                //4 additional methods for loyalty system
//                if (operation == "burntokens")
//                {
//                    if (args.length != 2) return 0;
//                    byte[] from = (byte[])args[0];
//                    biginteger value = (biginteger)args[1];
//                    return burnfrom(from, value);
//                }
//                if (operation == "minttokensto")
//                {
//                    if (args.length != 2) return 0;
//                    byte[] to = (byte[])args[0];
//                    biginteger value = (biginteger)args[1];
//                    return minttokensto(to, value);
//                }
//                if (operation == "usetokens")
//                {
//                    if (args.length != 2) return 0;
//                    byte[] from = (byte[])args[0];
//                    biginteger value = (biginteger)args[1];
//                    return usetokens(from, value);
//                }

//                if (operation == "totalused") return totalused();
//                if (operation == "totalburned") return totalburned();

//                if (operation == "decimals") return decimals();
//            }


//            return false;
//        }

//        public static object burnfrom(byte[] from, biginteger amount)
//        {

//            if (amount <= 0) return false;

//            //this method can only be executed by the owner of the smart contract or any admin.
//            //this method requires using "sendrawtransaction" so we check the signature here. 
//            if (!runtime.checkwitness(owner)) return false;

//            //check the balance of the address first
//            biginteger balanceof = storage.get(storage.currentcontext, from).asbiginteger();

//            //if the balance if less than a burning amount
//            if (balanceof < amount)
//            {
//                return false;
//            }

//            storage.put(storage.currentcontext, from, balanceof - amount); //subtract with the given amount


//            biginteger totalsupply = storage.get(storage.currentcontext, "totalsupply").asbiginteger();
//            storage.put(storage.currentcontext, "totalsupply", totalsupply - amount); //subtract with the given amount

//            //update totalburn
//            biginteger totalburn = storage.get(storage.currentcontext, "totalburned").asbiginteger();
//            storage.put(storage.currentcontext, "totalburned", totalburn - amount); //subtract with the given amount

//            //runtime.notify and event
//            return true;
//        }

//        //mint tokens to the address
//        public static bool minttokensto(byte[] to, biginteger amount)
//        {
//            if (amount <= 0) return false;

//            //this method can only be executed by the owner of the smart contract or any admin.
//            //this method requires using "sendrawtransaction" so we check the signature here. 
//            if (!runtime.checkwitness(owner)) return false;

//            //add balance to the address
//            biginteger balance = storage.get(storage.currentcontext, to).asbiginteger();
//            storage.put(storage.currentcontext, to, balance + amount);

//            //increase the totalsupply
//            biginteger totalsupply = storage.get(storage.currentcontext, "totalsupply").asbiginteger();
//            storage.put(storage.currentcontext, "totalsupply", amount + totalsupply);
//            transferred(null, to, amount);
//            return true;
//        }

//        // get the total token supply
//        // 获取已发行token总量
//        public static biginteger totalsupply()
//        {
//            return storage.get(storage.currentcontext, "totalsupply").asbiginteger();
//        }

//        // function that is always called when someone wants to transfer tokens.
//        // 流转token调用
//        public static bool transfer(byte[] from, byte[] to, biginteger value)
//        {
//            if (value <= 0) return false;
//            if (!runtime.checkwitness(from)) return false;
//            if (from == to) return true;
//            biginteger from_value = storage.get(storage.currentcontext, from).asbiginteger();
//            if (from_value < value) return false;
//            if (from_value == value)
//                storage.delete(storage.currentcontext, from);
//            else
//                storage.put(storage.currentcontext, from, from_value - value);
//            biginteger to_value = storage.get(storage.currentcontext, to).asbiginteger();
//            storage.put(storage.currentcontext, to, to_value + value);
//            transferred(from, to, value);
//            return true;
//        }

//        //use tokens
//        //subtract from sender
//        //subtract from totalsupply
//        //add to totalused
//        public static bool usetokens(byte[] from, biginteger value)
//        {
//            if (value <= 0) return false;
//            if (!runtime.checkwitness(from)) return false;

//            //subtract the amount from sender
//            biginteger from_value = storage.get(storage.currentcontext, from).asbiginteger();
//            if (from_value < value) return false;
//            if (from_value == value)
//                storage.delete(storage.currentcontext, from);
//            else
//                storage.put(storage.currentcontext, from, from_value - value);

//            //subtract the totalsupply
//            biginteger totalsupply = storage.get(storage.currentcontext, "totalsupply").asbiginteger();
//            storage.put(storage.currentcontext, "totalsupply", totalsupply - value); //subtract with the given amount

//            //add the totalused
//            biginteger totalused = storage.get(storage.currentcontext, "totalused").asbiginteger();
//            storage.put(storage.currentcontext, "totalused", totalused + value); //add with the given amount

//            return true;
//        }

//        public static biginteger totalused()
//        {
//            return storage.get(storage.currentcontext, "totalused").asbiginteger();
//        }

//        public static biginteger totalburned()
//        {
//            return storage.get(storage.currentcontext, "totalburned").asbiginteger();
//        }

//        // get the account balance of another account with address
//        public static biginteger balanceof(byte[] address)
//        {
//            return storage.get(storage.currentcontext, address).asbiginteger();
//        }

//    }
//}