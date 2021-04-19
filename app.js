const {parsed, error} = require('dotenv').config();

if (error) {
  console.log('dotenv error', error)
  throw error
} else {
  console.log('Loaded env variables')
  Object.keys(parsed).forEach(k => {
    console.log(`${k}: ${parsed[k]}`)
  })
  console.log('\nMachine-specific env variables')
  Object.keys(parsed).forEach(k => {
    console.log(`${k}: ${process.env[k]}`)
  })
  console.log('\nOverride ALREADY_SET_VAR with loaded variable')
  process.env.ALREADY_SET_VAR = parsed.ALREADY_SET_VAR
  console.log(`process.env.ALREADY_SET_VAR=${process.env.ALREADY_SET_VAR}`)
}

var http = require("http");
var express = require('express');
var app = express();

var mysql = require('mysql');

var bodyParser = require('body-parser');


var connection = mysql.createConnection(
{
	host     : process.env.RDS_HOSTNAME,
	user     : process.env.RDS_USERNAME,
	password : process.env.RDS_PASSWORD,
	port     : process.env.RDS_PORT,
	database : process.env.RDS_DB_NAME,
    dateStrings: 'date',
    multipleStatements: true

});

connection.connect(function(err){
	if(err) throw err
	console.log('connection...');
});

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
	extended : true
}));

var server = app.listen(3000, "0.0.0.0" , function(){
	var host = server.address().address;
	var port = server.address().port;

	console.log("example app listening at http://%s:%s" , host, port);
});



//#region Update

app.post('/userInfo/update' , function(req,res)
{
    var userInfo = req.body;
	var nickname = userInfo.nickname;
	
	var sql = 'update UserInfo SET ? where nickname = ?';
	connection.query(sql,[userInfo,nickname],function(error, results, fields)
	{	
		if(error){
			console.log(error);
			res.status(400).send(error);
		}
		else{
			console.log(results);

			res.status(200).send(JSON.stringify(results));
		}
				
	});
})

app.post('/userHistory/update' , function(req,res)
{
    var userHistory = req.body;
	var nickname = userHistory.nickname;
	
	var sql = 'update UserHistory SET ? where nickname = ?';
	connection.query(sql,[userHistory,nickname],function(error, results, fields)
	{	
		if(error){
			console.log(error);
			res.status(400).send(error);
		}
		else{
			console.log(results);

			res.status(200).send(JSON.stringify(results));
		}
				
	});
})

app.post('/userStage/update' , function(req,res)
{
    var userStage = req.body;
	var nickname = userStage.nickname;
	var stage_num = userStage.stage_num;

	var sql = 'update UserStage SET ? where nickname = ? and stage_num = ?';
	connection.query(sql,[userStage,nickname,stage_num],function(error, results, fields)
	{	
		if(error){
			console.log(error);
			res.status(400).send(error);
		}
		else{
			console.log(results);

			res.status(200).send(JSON.stringify(results));
		}
				
	});
})

app.post('/userInventory/update' , function(req,res)
{
    var userInventory = req.body;
	var nickname = userStage.nickname;
	var item_name = userStage.item_name;

	var sql = 'update UserInventory SET ? where nickname = ? and item_name = ?';
	connection.query(sql,[userInventory,nickname,item_name],function(error, results, fields)
	{	
		if(error){
			console.log(error);
			res.status(400).send(error);
		}
		else{
			console.log(results);

			res.status(200).send(JSON.stringify(results));
		}
				
	});
})

app.post('/userFriend/update' , function(req,res)
{
    var userFriend = req.body;
	var nickname_mine = userFriend.nickname_mine;
	var nickname_friend = userFriend.nickname_friend;

	var sql = 'update UserFriend SET ? where nickname_mine = ? and nickname_friend = ?';
	connection.query(sql,[userFriend,nickname_mine,nickname_friend],function(error, results, fields)
	{	
		if(error){
			console.log(error);
			res.status(400).send(error);
		}
		else{
			console.log(results);

			res.status(200).send(JSON.stringify(results));
		}
				
	});
})

app.post('/sendmailbox/update' , function(req,res)
{
    var mailbox = req.body;
	var nickname_mine = mailbox.sender;
	var nickname_friend = mailbox.receiver;

	var sql = 'insert into Mailbox set ?; update UserFriend set friendship = friendship + 1 , send = true where nickname_mine = ? and nickname_friend = ?';
	connection.query(sql,[mailbox,nickname_friend,nickname_mine,nickname_friend],function(error, results, fields)
	{	
		if(error){
			console.log(error);
			res.status(400).send(error);
		}
		else{
			console.log(results);

			res.status(200).send(JSON.stringify(results));
		}
				
	});
})

app.post('/getmailbox/update' , function(req,res)
{
    var mailbox = req.body;
	var sender = mailbox.sender;
	var receiver = mailbox.receiver;
    var time = mailbox.time;

	var sql = 'delete from Mailbox where receiver = ? and sender = ? and time = ?; update UserFriend set friendship = friendship + 1 where nickname_mine = ? and nickname_friend = ?';
	connection.query(sql,[mailbox,receiver,sender,time,receiver,sender],function(error, results, fields)
	{	
		if(error){
			console.log(error);
			res.status(400).send(error);
		}
		else{
			console.log(results);

			res.status(200).send(JSON.stringify(results));
		}
				
	});
})
//#endregion

//#region INSERT

app.post('/userInfo/insert' , function(req,res)
{
	var newUser = req.body;

	var sql = 'insert into UserInfo SET ?';

	connection.query(sql,newUser,function(error, results, fields)
	{	
		if(error){
			console.log(error);
			res.status(400).send(error);
		}
		else{
			console.log(results);
			res.status(200).send(JSON.stringify(results));
		}
				
	});

	
})

app.post('/userHistory/insert' , function(req,res)
{
	var newHistory = req.body;

	var sql = 'insert into UserHistory SET ?';

	connection.query(sql,newHistory,function(error, results, fields)
	{	
		if(error){
			console.log(error);
			res.status(400).send(error);
		}
		else{
			console.log(results);
			res.status(200).send(JSON.stringify(results));
		}
				
	});
})

app.post('/userFriend/insert' , function(req,res)
{
	var newFriend = req.body;

	var sql = 'insert into UserFriend SET ?';

	connection.query(sql,newFriend,function(error, results, fields)
	{	
		if(error){
			console.log(error);
			res.status(400).send(error);
		}
		else{
			console.log(results);
			res.status(200).send(JSON.stringify(results));
		}
				
	});

	
})

app.post('/userReward/insert' , function(req,res)
{
	var newReward = req.body;

	var sql = 'insert into UserReward SET ?';

	connection.query(sql,newReward,function(error, results, fields)
	{	
		if(error){
			console.log(error);
			res.status(400).send(error);
		}
		else{
			console.log(results);
			res.status(200).send(JSON.stringify(results));
		}
				
	});

	
})

app.post('/userStage/insert' , function(req,res)
{
	var newStage = req.body;

	var sql = 'insert into UserStage SET ?';

	connection.query(sql,newStage,function(error, results, fields)
	{	
		if(error){
			console.log(error);
			res.status(400).send(error);
		}
		else{
			console.log(results);
			res.status(200).send(JSON.stringify(results));
		}
				
	});

	
})

app.post('/userInventory/insert' , function(req,res)
{
	var newItem = req.body;

	var sql = 'insert into UserInventory SET ?';

	connection.query(sql,newItem,function(error, results, fields)
	{	
		if(error){
			console.log(error);
			res.status(400).send(error);
		}
		else{
			console.log(results);
			res.status(200).send(JSON.stringify(results));
		}
				
	});

	
})

app.post('/editorMap/insert' , function(req,res)
{
	var editorMap = req.body;

	var sql = 'insert into EditorMap SET ?';

	connection.query(sql,editorMap,function(error, results, fields)
	{	
		if(error){
			console.log(error);
			res.status(400).send(error);
		}
		else{
			console.log(results);
			res.status(200).send(JSON.stringify(results));
		}
				
	});

	
})


//#endregion

//#region GET

app.get('/userInfo/get' , function(req,res){
	
	var nickname = req.query.nickname;
	var sql = 'select * from UserInfo where nickname = ?';

	connection.query(sql,[nickname],function(error, results, fields)
	{	
		if(error)
        {
            console.log(error);
            res.status(400).send(error);
        }
        else
        {
            console.log("Info data : " + results);
		    res.status(200).send(JSON.stringify(results));		
        }
		
	});
})

app.get('/userHistory/get' , function(req,res){
	
	var nickname = req.query.nickname;
	var sql = 'select * from UserHistory where nickname = ?';

	connection.query(sql,[nickname],function(error, results, fields)
	{	
		if(error)
        {
            console.log(error);
            res.status(400).send(error);
        }
        else
        {
            console.log("History data : " + results);
		    res.status(200).send(JSON.stringify(results));		
        }
		
	});
})

app.get('/userReward/get' , function(req,res){
	
	var nickname = req.query.nickname;
	var sql = 'select * from UserReward where nickname = ?';

	connection.query(sql,[nickname],function(error, results, fields)
	{	
		if(error)
        {
            console.log(error);
            res.status(400).send(error);
        }
        else
        {
            console.log(results);
		    res.status(200).send(JSON.stringify(results));		
        }
		
	});
})

app.get('/userStage/get' , function(req,res){
	
	var nickname = req.query.nickname;
	var sql = 'select * from UserStage where nickname = ?';

	connection.query(sql,[nickname],function(error, results, fields)
	{	
		if(error)
        {
            console.log(error);
            res.status(400).send(error);
        }
        else
        {
            console.log("stage data : " + results);
		    res.status(200).send(JSON.stringify(results));		
        }
		
	});
})

app.get('/userInventory/get' , function(req,res){
	
	var nickname = req.query.nickname;
	var sql = 'select * from UserInventory where nickname = ?';
	
	connection.query(sql,[nickname],function(error, results, fields)
	{	
		if(error)
        {
            console.log(error);
            res.status(400).send(error);
        }
        else
        {
            console.log(results);
		    res.status(200).send(JSON.stringify(results));		
        }
		
	});
})

app.get('/userFriend/get' , function(req,res){
	
	var nickname_mine = req.query.nickname_mine;
	var sql = 'select * from UserFriend where nickname_mine = ?';

	connection.query(sql,[nickname_mine],function(error, results, fields)
	{	
		if(error)
        {
            console.log(error);
            res.status(400).send(error);
        }
        else
        {
            console.log("friend data : " + results);
		    res.status(200).send(JSON.stringify(results));		
        }
		
	});
})

app.get('/allUser/get' , function(req,res){

	var sql = 'select * from UserInfo';
	
	connection.query(sql,function(error, results, fields)
	{	
		if(error)
        {
            console.log(error);
            res.status(400).send(error);
        }
        else
        {
            console.log(results);
		    res.status(200).send(JSON.stringify(results));		
        }
		
	});
})

app.get('/editorMap/get' , function(req,res){

	var sql = 'select * from EditorMap';
	
	connection.query(sql,function(error, results, fields)
	{	
		if(error)
        {
            console.log(error);
            res.status(400).send(error);
        }
        else
        {
            console.log(results);
		    res.status(200).send(JSON.stringify(results));		
        }
		
	});
})

app.get('/mailbox/get' , function(req,res){
	
	var receiver = req.query.nickname;
	var sql = 'select * from Mailbox where receiver = ?';

	connection.query(sql,[receiver],function(error, results, fields)
	{	
		if(error)
        {
            console.log(error);
            res.status(400).send(error);
        }
        else
        {
            console.log("friend data : " + results);
		    res.status(200).send(JSON.stringify(results));		
        }
		
	});
})

app.get('/getall/get' , function(req,res){
	
	var nickname = req.query.nickname;
	var sql 
    ='select * from UserInfo where nickname = ?;'
    +'select * from UserHistory where nickname = ?;'
    +'select * from UserStage where nickname = ?;'
    +'select * from UserInventory where nickname = ?;'
    +'select * from UserFriend where nickname_mine = ?;'
    +'select * from UserReward where nickname = ?;'
    +'select * from Mailbox where receiver = ?';

	connection.query(sql,[nickname,nickname,nickname,nickname,nickname,nickname,nickname],function(error, results, fields)
	{	
		if(error)
        {
            console.log(error);
            res.status(400).send(error);
        }
        else
        {
		    res.status(200).send(JSON.stringify(results[0]));
            res.status(200).send(JSON.stringify(results[1]));		
            res.status(200).send(JSON.stringify(results[2]));		
            res.status(200).send(JSON.stringify(results[3]));		
            res.status(200).send(JSON.stringify(results[4]));		
            res.status(200).send(JSON.stringify(results[5]));		
            res.status(200).send(JSON.stringify(results[6]));				
        }
		
	});
})

app.post('/newUser/create' , function(req,res){
	
	var userInfo = req.body.userInfo;
    var userHistory = req.body.userHistory;
	var sql 
    ='insert into UserInfo set ?;'
    +'insert into UserHistory set ?';

	connection.query(sql,[userInfo,userHistory],function(error, results, fields)
	{	
		if(error)
        {
            console.log(error);
            res.status(400).send(error);
        }
        else
        {
            console.log("friend data : " + results);
		    res.status(200).send(JSON.stringify(results));		
        }
		
	});
})

//#endregion

//#region DELETE

app.post('/friend/delete',function(req,res){

	var id = req.body.nickname_mine;
	var friend_id = req.body.nickname_friend;

	
	//delete
	var delete_sql = 'delete from UserFriend where id = ? and friend_id =?; delete from UserFriend where id = ? and friend_id =?';

	connection.query(delete_sql,[id,friend_id,friend_id,id],function(error,results,fields){
		
				if(error)
				{
					console.log(error);
					res.status(400).send(error);
				}
				else
				{
					console.log(results);
					res.status(200).send(results);
				}
			
	})

	
})

//#endregion DELETE