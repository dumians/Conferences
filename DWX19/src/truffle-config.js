const fs = require('fs');

//var HDWalletProvider = require("truffle-hdwallet-provider");

// 12-word mnemonic
var mnemonic = process.env.NMEMONIC;

module.exports = {
  networks: {
    development: {
      host: "127.0.0.1",
      port: 8545,
      network_id: "*",
      gas: 4700000,
      gasPrice: 20000000000,
      websockets: true
    },
    "localhost:8545": {
      network_id: "*",
      port: 8545,
      host: "127.0.0.1",
      consortium_id: 1561347718833
    },
    dwx19: {
      network_id: "*",
      gas: 0,
      gasPrice: 0,
      provider: new HDWalletProvider(fs.readFileSync('c:\\HomeProjects\\Conf\\dwx19\\src\\postbox.env', 'utf-8'), "https://dwx19.blockchain.azure.com:3200/H8mYTyhdkumn3GmyqS7SPwrp"),
      consortium_id: 1561367031780
    }
  },
  live: {
    host: "178.25.19.88",
    port: 80,
    network_id: 1
  }
};
