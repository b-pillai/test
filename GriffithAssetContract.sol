pragma solidity ^0.4.18;

contract GriffithAssetContract {
    
   string ownerName;
   string vehicleType;
   uint makeYear;

   event assetAdded(
        string name,
        string vtype,
        uint year
    );
   
   function addAsset(string _oName,string _type, uint _year)  public {
       ownerName = _oName;
       vehicleType = _type;
       makeYear = _year;
       
       assetAdded(_oName, _type, _year);
   }
   
   function readAsset() public constant returns (string,string, uint) {
       return (ownerName, vehicleType, makeYear);
   }
    
}