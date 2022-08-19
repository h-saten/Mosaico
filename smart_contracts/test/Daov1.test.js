// const BigNumber = web3.BigNumber;
// require('chai').use(require('chai-bignumber')(BigNumber)).should();
// var utils = web3.utils;
// require('truffle-test-utils').init();
// const DAOv1 = artifacts.require("Daov1");
// const MosaicoERC20v1 = artifacts.require("MosaicoERC20v1");
// const truffleAssert = require('truffle-assertions');
// const blockchainTime = require("./helpers/truffleTimeHelper");

// contract('Daov1', accounts => {
//     const daoSettings = {
//         name: 'Mosaico',
//         isVotingEnabled: true,
//         onlyOwnerProposals: false,
//         initialVotingDelay: 20,
//         initialVotingPeriod: 50,
//         quorum: 4,
//         owner: accounts[0]
//     };

//     let daoContract;


//     const ercSettings = {
//         name: "Mosaico ERC20 Test Token",
//         symbol: "MOSTT",
//         initialSupply: 1000000,
//         isMintable: true,
//         isBurnable: true,
//         isPaused: false,
//         isGovernance: true
//     };
//     const _decimals = 18;

//     let ercContract;

//     beforeEach(async function () {
//         daoContract = await DAOv1.new(daoSettings);
//         ercSettings.walletAddress = daoContract.address;
//         ercContract = await MosaicoERC20v1.new(ercSettings);
//     });

//     describe('DAO: Basics', function() {

//         it('should deploy with proper name', async () => {
//             const daoName = await daoContract.name();
//             daoName.should.equal('Mosaico');
//         });

//         it('should deploy with proper voting enabled', async () => {
//             const isVotingEnabled = await daoContract.isVotingEnabled();
//             isVotingEnabled.should.equal(true);
//         });

//         it('should deploy with proper everybody can vote', async () => {
//             const onlyOwnerProposals = await daoContract.onlyOwnerProposals();
//             onlyOwnerProposals.should.equal(false);
//         });

//         it('should deploy with proper quorum', async () => {
//             const quorum = await daoContract.quorum();
//             quorum.toNumber().should.equal(4);
//         });

//         it('should deploy with proper voting delay', async () => {
//             const votingDelay = await daoContract.votingDelay();
//             votingDelay.toNumber().should.equal(20);
//         });

//         it('should deploy with proper voting period', async () => {
//             const votingPeriod = await daoContract.votingPeriod();
//             votingPeriod.toNumber().should.equal(50);
//         });
//     });

//     describe('DAO: Ownership', function() {
//         it('should have single owner after deployment', async () => {
//             const owners = await daoContract.getOwners();
//             Array.isArray(owners).should.equal(true);
//             owners.length.should.equal(1);
//             owners[0].should.eq(accounts[0]);
//         });

//         it('should have single approver after deployment', async () => {
//             const approvals = await daoContract.requiredApprovals();
//             approvals.toNumber().should.eq(1);
//         });

//         it('should add new owner', async () => {
//             await daoContract.addOwner(accounts[1]);
//             const owners = await daoContract.getOwners();
//             Array.isArray(owners).should.equal(true);
//             owners.length.should.equal(2);
//             owners[0].should.eq(accounts[0]);
//             owners[1].should.eq(accounts[1]);
//         });

//         it('should remove owner', async () => {
//             await daoContract.addOwner(accounts[1]);
//             var owners = await daoContract.getOwners();
//             Array.isArray(owners).should.equal(true);
//             owners.length.should.equal(2);
//             owners[0].should.eq(accounts[0]);
//             owners[1].should.eq(accounts[1]);
//             await daoContract.removeOwner(accounts[0], {from: accounts[1]});
//             var owners = await daoContract.getOwners();
//             Array.isArray(owners).should.equal(true);
//             owners.length.should.equal(1);
//             owners[0].should.eq(accounts[1]);
//         });

//         it('should fail when adding owner without rights', async () => {
//             try {
//                 await daoContract.addOwner(accounts[1], {from: accounts[2]});
//                 throw "success";
//             }
//             catch (error) {
//                 error.should.not.equal("success");
//             }
//         });

//         it('should fail when adding owner again', async () => {
//             try {
//                 await daoContract.addOwner(accounts[1]);
//                 await daoContract.addOwner(accounts[1]);
//                 throw "success";
//             }
//             catch (error) {
//                 error.should.not.equal("success");
//             }
//         });
//     });

//     describe('DAO: Token Management', function() {
//         it('should add token', async () => {
//             await daoContract.addToken(ercContract.address, true);
//             const tokens = await daoContract.getTokens();
//             Array.isArray(tokens).should.equal(true);
//             tokens.length.should.equal(1);
//         });

//         it('should have all tokens', async () => {
//             await daoContract.addToken(ercContract.address, true);
//             const balance = await ercContract.balanceOf(daoContract.address);
//             balance.toNumber().should.equal(ercSettings.initialSupply);
//         });

//         it('should fail when adding token again', async () => {
//             try {
//                 await daoContract.addToken(ercContract.address, true);
//                 await daoContract.addToken(ercContract.address, true);
//                 throw "success";
//             }
//             catch (error) {
//                 error.should.not.equal("success");
//             }
//         });

//         it('should mint', async () => {
//             await daoContract.addToken(ercContract.address, true);
//             await daoContract.mint(ercContract.address, 1000);
//             const balance = await ercContract.balanceOf(daoContract.address);
//             balance.toNumber().should.equal(ercSettings.initialSupply + 1000);
//         });

//         it('should fail if minting not managed token', async () => {
//             try{
//                 await daoContract.mint(ercContract.address, 1000);
//                 throw "success";
//             }
//             catch(error) {
//                 error.should.not.equal("success");
//             }
//         });

//         it('should burn', async () => {
//             await daoContract.addToken(ercContract.address, true);
//             await daoContract.burn(ercContract.address, 1000);
//             const balance = await ercContract.balanceOf(daoContract.address);
//             balance.toNumber().should.equal(ercSettings.initialSupply - 1000);
//         });

//         it('should fail if burning not managed token', async () => {
//             try{
//                 await daoContract.burn(ercContract.address, 1000);
//                 throw "success";
//             }
//             catch(error) {
//                 error.should.not.equal("success");
//             }
//         });
//     });

//     describe('DAO: Voting', function () {
//         it('should propose', async () => {
//             await daoContract.addToken(ercContract.address, true);
//             const proposal = await daoContract.propose("Test", ercContract.address);
//             truffleAssert.eventEmitted(proposal, 'ProposalCreated', (ev) => {
//                 return ev.proposalId.toString().should.have.lengthOf.above(1);
//             });
//         });

//         it('state should be pending', async () => {
//             await daoContract.addToken(ercContract.address, true);
//             await daoContract.propose("Test", ercContract.address);
//             const proposalId = await daoContract.hashProposal("Test", ercContract.address);
//             const state = await daoContract.state(proposalId);
//             state.toNumber().should.equal(0);
//         });

//         it('should not vote non-active', async () => {
//             await daoContract.addToken(ercContract.address, true);
//             await daoContract.propose("Test", ercContract.address);
//             const proposalId = await daoContract.hashProposal("Test", ercContract.address);
//             try{
//                 await daoContract.castVote(proposalId, 0);
//                 throw "success";
//             }
//             catch(error){
//                 error.should.not.equal("success");
//             }
//         });

//         it('should not vote insufficient tokens', async () => {
//             await daoContract.addToken(ercContract.address, true);
//             await daoContract.propose("Test", ercContract.address);
//             const proposalId = await daoContract.hashProposal("Test", ercContract.address);
//             try{
//                 await daoContract.castVote(proposalId, 0);
//                 throw "success";
//             }
//             catch(error){
//                 error.should.not.equal("success");
//             }
//         });

//         it('state should be active', async () => {
//             await daoContract.addToken(ercContract.address, true);
//             await daoContract.propose("Test", ercContract.address);
//             const proposalId = await daoContract.hashProposal("Test", ercContract.address);
//             console.log("Current block: " + (await web3.eth.getBlock('latest')).number);
//             await blockchainTime.advanceTimeAndBlock(30);
//             console.log("Current block after increase: " + (await web3.eth.getBlock('latest')).number);
//             const state = await daoContract.state(proposalId);
//             state.toNumber().should.equal(1);
//         });

//         it('should fail when propose invalid token', async () => {
//             try {
//                 const proposalId = await daoContract.propose("Test Intention", ercContract.address);
//                 throw "success";
//             }
//             catch (error) {
//                 error.should.not.equal("success");
//             }
//         });

//         it('should not vote because not active', async () => {
//             await daoContract.addToken(ercContract.address, true);
//             await daoContract.propose("Test", ercContract.address);
//             const proposalId = await daoContract.hashProposal("Test", ercContract.address);
//             try{
//                 await daoContract.castVote(proposalId, utils.toBN(0));
//                 throw "success";
//             }
//             catch(error) {
//                 error.should.not.equal("success");
//             }
//         });

//         it('should not vote because no tokens', async () => {
//             await daoContract.addToken(ercContract.address, true);
//             await daoContract.propose("Test", ercContract.address);
//             const proposalId = await daoContract.hashProposal("Test", ercContract.address);
//             await blockchainTime.advanceTimeAndBlock(30);
//             try{
//                 await daoContract.castVote(proposalId, utils.toBN(0));
//                 throw "success";
//             }
//             catch(error) {
//                 error.should.not.equal("success");
//             }
//         });

//         it('should not vote because closed', async () => {
//             await daoContract.addToken(ercContract.address, true);
//             await daoContract.propose("Test", ercContract.address);
//             const proposalId = await daoContract.hashProposal("Test", ercContract.address);
//             await blockchainTime.advanceTimeAndBlock(90);
//             try{
//                 await daoContract.castVote(proposalId, utils.toBN(0));
//                 throw "success";
//             }
//             catch(error) {
//                 error.should.not.equal("success");
//             }
//         });

//         it('should vote against', async () => {
//             await daoContract.addToken(ercContract.address, true);
//             await daoContract.propose("Test", ercContract.address);
//             const proposalId = await daoContract.hashProposal("Test", ercContract.address);
//             await daoContract.submitTransaction(accounts[0], 20, web3.utils.asciiToHex("empty"), 1, ercContract.address);
//             await daoContract.executeTransaction(0);
//             await blockchainTime.advanceTimeAndBlock(30);
//             await daoContract.castVote(proposalId, utils.toBN(0));
//             const isVoted = await daoContract.hasVoted(proposalId, accounts[0]);
//             isVoted.should.eq(true);
//             const votes = await daoContract.proposalVotes(proposalId);
//             votes.againstVotes.toNumber().should.eq(20);
//             const state = await daoContract.state(proposalId);
//             state.toNumber().should.equal(1);
//         });

//         it('should defeat', async () => {
//             await daoContract.addToken(ercContract.address, true);
//             await daoContract.propose("Test", ercContract.address);
//             const proposalId = await daoContract.hashProposal("Test", ercContract.address);
//             await daoContract.submitTransaction(accounts[0], 1000000 * 0.5, web3.utils.asciiToHex("empty"), 1, ercContract.address);
//             await daoContract.executeTransaction(0);
//             await blockchainTime.advanceTimeAndBlock(30);
//             await daoContract.castVote(proposalId, utils.toBN(0));
//             await blockchainTime.advanceTimeAndBlock(60);
//             const state = await daoContract.state(proposalId);
//             state.toNumber().should.equal(3);
//         });

//         it('should succeed', async () => {
//             await daoContract.addToken(ercContract.address, true);
//             await daoContract.propose("Test", ercContract.address);
//             const proposalId = await daoContract.hashProposal("Test", ercContract.address);
//             await daoContract.submitTransaction(accounts[0], 1000000 * 0.5, web3.utils.asciiToHex("empty"), 1, ercContract.address);
//             await daoContract.executeTransaction(0);
//             await blockchainTime.advanceTimeAndBlock(30);
//             await daoContract.castVote(proposalId, utils.toBN(1));
//             await blockchainTime.advanceTimeAndBlock(60);
//             const state = await daoContract.state(proposalId);
//             state.toNumber().should.equal(4);
//         });

//         it('should have proper quorum', async () => {
//             await daoContract.addToken(ercContract.address, true);
//             const quorum = await daoContract.tokenQuorum(ercContract.address);
//             quorum.toNumber().should.equal(1000000 * 0.04);
//         });

//         it('should not propose if not voting token', async () => {
//             await daoContract.addToken(ercContract.address, false);
//             try{
//                 const proposal = await daoContract.propose("Test", ercContract.address);
//                 throw "success";
//             }
//             catch(error){
//                 error.should.not.equal("success");
//             }
//         });

//         it('should not propose if voting disabled', async () => {
//             const newDaoSettings = {...daoSettings, isVotingEnabled: false};
//             daoContract = await DAOv1.new(newDaoSettings);
//             ercSettings.walletAddress = daoContract.address;
//             ercContract = await MosaicoERC20v1.new(ercSettings);
//             await daoContract.addToken(ercContract.address, false);
//             try{
//                 const proposal = await daoContract.propose("Test", ercContract.address);
//                 throw "success";
//             }
//             catch(error){
//                 error.should.not.equal("success");
//             }
//         });

//         it('should not propose if not owner', async () => {
//             const newDaoSettings = {...daoSettings, isVotingEnabled: true, onlyOwnerProposals: true};
//             daoContract = await DAOv1.new(newDaoSettings);
//             ercSettings.walletAddress = daoContract.address;
//             ercContract = await MosaicoERC20v1.new(ercSettings);
//             await daoContract.addToken(ercContract.address, false);
//             try{
//                 const proposal = await daoContract.propose("Test", ercContract.address, {from: accounts[1]});
//                 throw "success";
//             }
//             catch(error){
//                 error.should.not.equal("success");
//             }
//         });

//         it('should propose if not owner', async () => {
//             await daoContract.addToken(ercContract.address, true);
//             const proposal = await daoContract.propose("Test", ercContract.address, {from: accounts[1]});
//             truffleAssert.eventEmitted(proposal, 'ProposalCreated', (ev) => {
//                 return ev.proposalId.toString().should.have.lengthOf.above(1);
//             });
//         });
//     });
// });