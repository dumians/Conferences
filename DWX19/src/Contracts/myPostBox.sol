pragma solidity >=0.4.22 <0.6.0;

contract myPostBox{
    string message;
    int64 count;
    event Posted(string message);
    
    function postMsg(string memory text) public {
        emit Posted(text);
        count++;
        message = text;
    }
    
    function getMsg() public view returns (string memory) {
        return message;   
    }
    
    function getCount() public view returns (int64) {
        return count;
    }
}