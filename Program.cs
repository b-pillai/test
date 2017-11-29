using System;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Geth;
using Nethereum.Web3.Accounts.Managed;
using System.Threading;
using Nethereum.RPC.Accounts;
using Nethereum;
using System.Threading.Tasks;
using Nethereum.Web3.Accounts;
using System.Collections.Generic;

namespace message.contract
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            var _deployContract = new deployContract();
            _deployContract.deployContractUsingPrivateKey();
            while (!_deployContract.IsComplete)
            {
                Console.Write(".");
                Thread.Sleep(100);
            }
            Console.Write("Done thanks");
            Console.ReadKey();
        }

        /* 
        contract NameContract
        {
            string storedData;
            function NameContract(string myName){
                storedData = myName;
            }
            function setName(string name) returns(string){
                storedData = name;
                return storedData;
            }
            function get() constant returns(string){
                return storedData;
            }
        }
        */
        public class deployContract
        {
            public bool IsComplete { get; private set; }

            public async void deployContractUsingPrivateKey()
            {
                var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
                var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
                
                var abi = @"[{'constant':true,'inputs':[],'name':'get','outputs':[{'name':'','type':'string'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'name','type':'string'}],'name':'setName','outputs':[{'name':'','type':'string'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'inputs':[{'name':'myName','type':'string'}],'payable':false,'stateMutability':'nonpayable','type':'constructor'}]";
                var byteCode = "0x6060604052341561000f57600080fd5b6040516104ff3803806104ff833981016040528080518201919050508060009080519060200190610041929190610048565b50506100ed565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f1061008957805160ff19168380011785556100b7565b828001600101855582156100b7579182015b828111156100b657825182559160200191906001019061009b565b5b5090506100c491906100c8565b5090565b6100ea91905b808211156100e65760008160009055506001016100ce565b5090565b90565b610403806100fc6000396000f30060606040526004361061004c576000357c0100000000000000000000000000000000000000000000000000000000900463ffffffff1680636d4ce63c14610051578063c47f0027146100df575b600080fd5b341561005c57600080fd5b6100646101b5565b6040518080602001828103825283818151815260200191508051906020019080838360005b838110156100a4578082015181840152602081019050610089565b50505050905090810190601f1680156100d15780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b34156100ea57600080fd5b61013a600480803590602001908201803590602001908080601f0160208091040260200160405190810160405280939291908181526020018383808284378201915050505050509190505061025d565b6040518080602001828103825283818151815260200191508051906020019080838360005b8381101561017a57808201518184015260208101905061015f565b50505050905090810190601f1680156101a75780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b6101bd61031e565b60008054600181600116156101000203166002900480601f0160208091040260200160405190810160405280929190818152602001828054600181600116156101000203166002900480156102535780601f1061022857610100808354040283529160200191610253565b820191906000526020600020905b81548152906001019060200180831161023657829003601f168201915b5050505050905090565b61026561031e565b816000908051906020019061027b929190610332565b5060008054600181600116156101000203166002900480601f0160208091040260200160405190810160405280929190818152602001828054600181600116156101000203166002900480156103125780601f106102e757610100808354040283529160200191610312565b820191906000526020600020905b8154815290600101906020018083116102f557829003601f168201915b50505050509050919050565b602060405190810160405280600081525090565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f1061037357805160ff19168380011785556103a1565b828001600101855582156103a1579182015b828111156103a0578251825591602001919060010190610385565b5b5090506103ae91906103b2565b5090565b6103d491905b808211156103d05760008160009055506001016103b8565b5090565b905600a165627a7a7230582019551c4d7b09fb78721f988cab8583709121a84daf1df2d40490fd0aad906e8b0029";
                
                var myName = "babu";
                
                Console.WriteLine("start");

                var web3 = new Nethereum.Geth.Web3Geth(new Account(privateKey));
                
                Console.WriteLine("SenderAddress:- " + senderAddress);
                Console.WriteLine("PrivateKey:- " + privateKey);

                Nethereum.RPC.Eth.DTOs.TransactionReceipt receipt = null;

                //deploy the contract, set the name variable and get the receipt
                while (receipt == null)
                {
                    await Task.Delay(5000);
                    receipt = await web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(
                        abi, byteCode, senderAddress, new Nethereum.Hex.HexTypes.HexBigInteger(900000), null, myName);
                }
                // get the contract address from the receipt
                var contract = web3.Eth.GetContract(abi, receipt.ContractAddress);
                
                Console.WriteLine("");
                Console.WriteLine("Contract Address:- " + receipt.ContractAddress);

                //read the 'name' value from by calling the get function.
                var getFunction = contract.GetFunction("get");
                // this part is not working, the function dont take any arguments and returns a string.
                var getResult = getFunction.CallAsync<String>();//??? help please
                
                Console.WriteLine("Result:- " + getResult);

                //write new value to the 'name' variable by calling the setName function.
                var multiplyFunction = contract.GetFunction("setName");
                var result = await multiplyFunction.CallAsync<string>("pillai");

                IsComplete = true;
                Console.WriteLine("Result:- " + result);
                Console.ReadLine();

            }
        }

    }
}
