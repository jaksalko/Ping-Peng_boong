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


app.post('/newEditorStage/update' , function(req,res)
{
    var userInfo = req.body.userInfo;//update
	var userHistory = req.body.userHistory;//update
	var userStage = req.body.userStage;//insert
	var editorMap = req.body.editorMap;//update

	var nickname = userInfo.nickname;
	var map_id = editorMap.map_id;
	

	var sql = 'update UserInfo set ? where nickname =?;'
	+'update UserHistory set ? where nickname =?;'
	+'insert into UserStage set ?;'
	+'update EditorMap set ? where map_id = ?';
	connection.query(sql,[userInfo,nickname,userHistory,nickname,userStage,editorMap,map_id],function(error, results, fields)
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
app.post('/updateEditorStage/update' , function(req,res)
{
    var userInfo = req.body.userInfo;//update
	var userHistory = req.body.userHistory;//update
	var userStage = req.body.userStage;//update
	var editorMap = req.body.editorMap;//update

	var nickname = userInfo.nickname;
	var stage_num = userStage.stage_num;
	var map_id = editorMap.map_id;
	

	var sql = 'update UserInfo set ? where nickname =?;'
	+'update UserHistory set ? where nickname =?;'
	+'update UserStage set ? where nickname = ? and stage_num = ?;'
	+'update EditorMap set ? where map_id = ?';
	connection.query(sql,[userInfo,nickname,userHistory,nickname,userStage,nickname,stage_num,editorMap,map_id],function(error, results, fields)
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
app.post('/newStage/update' , function(req,res)
{
    var userInfo = req.body.userInfo;//update
	var userHistory = req.body.userHistory;//update
	var userStage = req.body.userStage;//insert

	var nickname = userInfo.nickname;

	

	var sql = 'update UserInfo set ? where nickname =?;'
	+'update UserHistory set ? where nickname =?;'
	+'insert into UserStage set ?'
	connection.query(sql,[userInfo,nickname,userHistory,nickname,userStage],function(error, results, fields)
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


//#region Update

app.post('/updateStage/update' , function(req,res)
{
    var userInfo = req.body.userInfo;//update
	var userHistory = req.body.userHistory;//update
	var userStage = req.body.userStage;//update

	var nickname = userInfo.nickname;
	var stage_num = userStage.stage_num;

	var sql = 'update UserInfo set ? where nickname =?;'
	+'update UserHistory set ? where nickname = ?;'
	+'update UserStage set ? where nickname = ? and stage_num = ?'
	connection.query(sql,[userInfo,nickname,userHistory,nickname,userStage,nickname,stage_num],function(error, results, fields)
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

app.post('/infoHistory/update' , function(req,res)
{
    var userHistory = req.body.userHistory;
	var userInfo = req.body.userInfo;

	var nickname = userHistory.nickname;
	
	var sql = 'update UserHistory SET ? where nickname = ?;'
	+'update UserInfo SET ? where nickname = ?';
	connection.query(sql,[userHistory,nickname,userInfo,nickname],function(error, results, fields)
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

app.post('/userFriend/update' , function(req,res)//accept request
{
    var request = req.body;
	
	
	var nickname_mine = request.nickname_mine;
	var nickname_friend = request.nickname_friend;


	//console.log(myRequest.nickname_mine + " "+myRequest.nickname_friend+" "+myState + "," + friendState);

	var sql = 'update UserFriend set state = 2 where nickname_mine = ? and nickname_friend = ?;'
	+'update UserFriend set state = 2 where nickname_mine = ? and nickname_friend = ?';
	connection.query(sql,[nickname_mine,nickname_friend,nickname_friend,nickname_mine],function(error, results, fields)
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

app.post('/editorMap/like/update' , function(req,res)
{
    var editorMap = req.body;
	var map_id = editorMap.map_id;
	

	var sql = 'update EditorMap set likes = likes + 1 where map_id = ?';
	connection.query(sql,[map_id],function(error, results, fields)
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

app.post('/skinBuy/update' , function(req,res)
{
	var userInfo = req.body.userInfo;//update
	var userHistory = req.body.userHistory;//update
	var item = req.body.item;//update

	var nickname = userInfo.nickname;
	

	var sql = 'update UserInfo set ? where nickname =?;'
	+'update UserHistory set ? where nickname = ?;'
	+'insert into UserInventory set ?'


	connection.query(sql,[userInfo,nickname,userHistory,nickname,item],function(error, results, fields)
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
    var mailbox = req.body.mailbox;
	var friend = req.body.myFriend;


	var nickname_mine = mailbox.sender;
	var nickname_friend = mailbox.receiver;

	var sql = 'insert into Mailbox set ?;'+
	'update UserFriend set ? where nickname_mine = ? and nickname_friend = ?;'
	+'update UserFriend set friendship = friendship + 1 where nickname_mine = ? and nickname_friend = ?';
	connection.query(sql,[mailbox,friend,nickname_mine,nickname_friend,nickname_friend,nickname_mine],function(error, results, fields)
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

	var sql = 'delete from Mailbox where receiver = ? and sender = ? and time = ?;'+
	'update UserInfo set heart = heart +1 where nickname = ?;'
	+'update UserHistory set heart_get = heart_get + 1 where nickname = ?';
	connection.query(sql,[receiver,sender,time,receiver,receiver],function(error, results, fields)
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
	var myRequest = req.body.myRequest;
	var friendRequest = req.body.friendRequest;
	var sql = 'insert into UserFriend SET ?;'
	+'insert into UserFriend SET ?';
	
	connection.query(sql,[myRequest,friendRequest],function(error, results, fields)
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
	var userInfo = req.body.info;
    var userHistory = req.body.history;
	var userInventory = req.body.inventory;
	var userReward = req.body.reward;
	var nickname = userInfo.nickname;
	var sql 
    ='update UserInfo set ? where nickname = ?;'
    +'update UserHistory set ? where nickname = ?;'
	+'insert into UserReward SET ?;';

	if(userInventory.item_name != "none")
	{
		console.log("item update");
		sql += 'insert into UserInventory SET ?';
		connection.query(sql,[userInfo,nickname,userHistory,nickname,userReward,userInventory],function(error, results, fields)
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
	}
	else
	{
		console.log("no item update");
		connection.query(sql,[userInfo,nickname,userHistory,nickname,userReward],function(error, results, fields)
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
	}

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
            
		    res.status(200).send(JSON.stringify(results));		
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

app.post('/userFriend/delete',function(req,res){

	var id = req.body.nickname_mine;
	var friend_id = req.body.nickname_friend;

	
	//delete
	var delete_sql = 'delete from UserFriend where nickname_mine = ? and nickname_friend =?; delete from UserFriend where nickname_mine = ? and nickname_friend =?';

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