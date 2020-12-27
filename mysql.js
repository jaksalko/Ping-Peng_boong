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
        host : process.env.DB_HOST,
        user : process.env.DB_USER,
        password : process.env.DB_PASSWORD,
        database : 'maplist'
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
app.get('/count',function(req,res){
	console.log(req);
	connection.query('select count(*) from map',function(error,result,field)
	{
		if(error){console.log(error);}
		res.end(JSON.stringify(result));
	});
});
app.get('/map/difficulty' , function(req,res){
	var param = req.query.difficulty;
	var nickname = req.query.nickname;
	var sql = 'select * from map where difficulty = ? and nickname != ?';
	console.log("param : "+ param+"  nick : " + nickname);
	connection.query(sql,[param,nickname],function(error, results, fields)
	{	
		if(error){console.log(error);}
		console.log(results);
		res.end(JSON.stringify(results));		
	});
});
app.get('/map/all' , function(req,res){
	
	var nickname = req.query.nickname;
	var sql = 'select * from map where nickname != ?';
	console.log("nick : " + nickname);
	connection.query(sql,[nickname],function(error, results, fields)
	{	
		if(error){console.log(error);}
		console.log(results);
		res.end(JSON.stringify(results));		
	});
});
app.post('/map/clear' , function(req,res)
{
	var title = req.body.title;
	var moveCount = req.body.moveCount;
	var difficulty = req.body.difficulty;
	var sql = 'update map set moveCount = ?,difficulty = ? where title = ?';
	connection.query(sql,[moveCount,difficulty,title],function(error, results, fields)
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
app.post('/map/play' , function(req,res)
{
	var title = req.body.title;
	var sql = 'update map set popularity = popularity + 1 where title = ?';
	connection.query(sql,[title],function(error, results, fields)
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
app.post('/map/push' , function(req,res)
{
	var title = req.body.title;

	var sql = 'update map set push = push + 1 where title = ?';
	connection.query(sql,[title],function(error, results, fields)
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

app.post('/editorPlay/add' , function(req,res)
{
	var postData = req.body;

	var sql = 'insert into editorPlay SET ?';

	connection.query(sql,postData,function(error, results, fields)
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
app.post('/editorPlay/push' , function(req,res)
{
	var title = req.body.title;

	var sql = 'update editorPlay set push = push + 1 where title = ?';
	connection.query(sql,[title],function(error, results, fields)
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
app.get('/editorPlay/all' , function(req,res){
	
	var player_id = req.query.player_id;
	var sql = 'select * from editorPlay where player_id != ?';
	console.log("player : " + player_id);
	connection.query(sql,[player_id],function(error, results, fields)
	{	
		if(error){console.log(error);}
		console.log(results);
		res.end(JSON.stringify(results));		
	});
});
app.get('/test' , function(req,res){
	console.log(req);
	connection.query('select * from map',function(error, results, fields)
	{	
		if(error){console.log(error);}
		res.end(JSON.stringify(results));		
	});
});
app.post('/editor/generate',function(req,res){
	var postData = req.body;
	connection.query('INSERT INTO map SET ?', postData ,
	 function(error,results,fields)
	{
		if(error){console.log(error);}
		res.end(JSON.stringify(results));
	});
});


//Account Setting
app.get('/account/checkid' , function(req,res){
	var id = req.query.id;
	var sql = 'select count(*) as idCount from user where id = ?';

	
	connection.query(sql,[id],function(error,results,fields){
		if(error){
			console.error(error);
			res.end('error');
		}
		else{
			

			var count = results[0].idCount;
			console.log(count);

			if(count == 0)
			{
				res.status(200).send(JSON.stringify(results));
			}
			else
			{
				res.status(204).send('alreay exist');
			}
		
			//res.send(JSON.stringify(results));
			

			//res.end(JSON.stringify(results));
		}
	})

});

app.post('/account/add',function(req,res){
	/*var id = req.query.id;
	var nickname = req.query.nickname;
	var cash = req.query.cash;

	var user = {'id':id,
	'nickname':nickname,
	'cash':cash};*/
	var postData = req.body;
	console.log("id :" + req.body.id +"nickname : " + req.body.nickname);

	connection.query('select count(*) as nickCount from user where nickname = ? or id =?',[req.body.nickname,req.body.id]
	,function(error , results , fields){
		if(error){
			console.error(error);
			res.end('error');
		}
		else{
			

			var count = results[0].nickCount;
			console.log(count);

			if(count == 0)
			{
				connection.query('insert into user set ?',postData,
				function(err,result,fields){
					if(err){
						console.error(err);
						res.end('error');
						
					}
					else
					{
						
						console.log(result);
						res.status(200).send(JSON.stringify(result));
					}
				
				});
			}
			else
			{
				res.status(204).send('alreay exist');
			}
			
			

			//res.end(JSON.stringify(results));
		}
	})

	
});

//Account update
app.post('/account/update' , function(req,res)
{
	var id = req.body.id;
	var change_cash = req.body.cash;
	var stage = req.body.stage;
	var change_heart = req.body.heart;
	var sql = 'update user set cash = cash + ?, heart = heart + ?, stage = ? where id = ?';
	connection.query(sql,[change_cash,change_heart,stage,id],function(error, results, fields)
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

app.get('/account/info' , function(req,res)
{
	var id = req.query.id;
	var sql = 'select * from user where id = ?';
	console.log("get info... ID : " + id);
	connection.query(sql,id , function(error,result, fields)
	{
		if(error)
		{
			console.log(error);
			res.status(400).send(error);
		}
		else
		{
			console.log(result);
			res.status(200).send(JSON.stringify(result));
		}
	})
})

app.post('/account/stage' , function(req,res)
{
	var id = req.body.id;
	var stage = req.body.stage;

	var sql = 'update user set stage = ? where id = ?';
	connection.query(sql,[stage,id],function(error,result,fields){
		if(error)
		{
			console.log(error);
			res.status(400).send(error);
		}
		else
		{
			console.log(result);
			res.status(200).send(JSON.stringify(result));
		}
	})
})

//Stage manage
app.post('/stage/update',function(req,res)
{
	var id = req.body.id;
	var stage_num = req.body.stage_num;
	var stage_step = req.body.stage_step;

	var sql = 'update stage set stage_step = ? where id = ? and stage_num = ? and stage_step > ?';
	connection.query(sql,[stage_step,id,stage_num,stage_step],function(error,result,fields){

		if(error)
		{
			console.log(error);
			res.status(400).send(error);
		}
		else
		{
			console.log(result);
			res.status(200).send(JSON.stringify(result));
		}
	})


})
app.post('/stage/insert' , function(req,res){

	var postData = req.body;//id stage_num stage_step

	var sql = 'INSERT INTO stage SET ?';

	connection.query(sql,postData,function(error,result,fields){
		if(error)
		{
			console.log(error);
			res.status(400).send(error);
		}
		else
		{
			console.log(result);
			res.status(200).send(JSON.stringify(result));
		}
	})

})

app.get('/stage/info',function(req,res){
	var id = req.query.id;
	var sql = 'select * from stage where id = ?';
	connection.query(sql,id,function(error,results,fields){
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
	})
})

//Friend Manage
app.post('/friend/send', function(req,res){
	var postData = req.body;

	sql = 'insert into friendrequest set ?';
	connection.query(sql,postData,function(error,results,fields){
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

app.get('/friend/request', function(req,res){
	var friend_id = req.query.id;

	sql = 'select * from friendrequest where friend_id = ?';
	connection.query(sql,friend_id,function(error,results,fields){
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
	})
})

app.get('/friend/list', function(req,res){
	var id = req.query.id;

	sql = 'select * from friend where friend_id = ? or id = ?';
	connection.query(sql,[id,id],function(error,results,fields){
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
	})
})

app.post('/friend/accept',function(req,res){
	var postData = req.body;
	var id = req.body.id;
	var friend_id = req.body.friend_id;

	//insert
	var insert_sql = 'insert into friend set ?';
	//delete
	var delete_sql = 'delete from friendrequest where id = ? and friend_id =?';

	connection.query(insert_sql,postData,function(error,results,fields){
		if(error)
		{
			console.log(error);
			res.status(400).send(error);
		}
		else
		{
			console.log(results);
			//res.status(200).send(results).end();

			connection.query(delete_sql,[id,friend_id],function(error,results,fields){
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

		}
	})

	
})
//app.post('/friend/decline')
//app.post('/friend/delete')

//chat
app.post('/chat/post',function(req,res){
	var postData = req.body;
	var id_1 = req.body.id_1;
	var id_2 = req.body.id_2;

	var sql = 'insert into chat set ?';
	connection.query(sql,postData,function(error,result,fields){
		if(error)
		{
			console.log(error);
			res.status(400).send(error);
		}
		else
		{
			/*var sql_select = 'select * from chat where (id_1 = ? and id_2 = ?) or (id_1 = ? and id_2 = ?)';
			connection.query(sql_select,[id_1,id_2,id_2,id_1],function(error,results,fields){
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
			})
			*/
			console.log(error);
			res.status(200).send('success');
		}
	})
})

app.get('/chat/get',function(req,res){
	var id_1 = req.query.id_1;
	var id_2 = req.query.id_2;

	var sql_select = 'select * from chat where (id_1 = ? and id_2 = ?) or (id_1 = ? and id_2 = ?)';
			connection.query(sql_select,[id_1,id_2,id_2,id_1],function(error,results,fields){
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
			})
})

//Store API
app.get('/igloo/playerskin', function(req,res){
	var userid = req.query.userid;
	var sql = 'select skinid_1, skinid_2, skinid_3, skinid_4, skinid_5 from skin where userid=?';
	console.log(sql + userid);
        connection.query(sql, userid, function(error, results, fields)
        {
                if(error){console.log(error);}
                res.end(JSON.stringify(results));
        });
});
app.post('/igloo/playerskin',function(req,res){
	var userid = req.body.userid;
        var skinid = req.body.skinid;
	console.log(userid);
	var existuser = false;
	connection.query('select distinct userid from skin', function(error, rows, fields)
	{
		for(var i = 0; i < rows.length; i++)
		{
			console.log(rows[i]);
			if(userid == rows[i].userid) {existuser = true;}
		}
		if(!existuser) {res.end("wrong user id");}
	});

	var existskin = false;
	var key1 = "default";
	var key2 = "default";
	var rid = 0;
	connection.query('select * from skin where userid=?', userid, function(error, rows, fields)
	{
		if(error) {console.log(error);}
		console.log(rows);
		for(var j = 0; j < rows.length; j++)
		{
			if(rows[j].skinid_1 == skinid ||
				rows[j].skinid_2 == skinid ||
				rows[j].skinid_3 == skinid ||
				rows[j].skinid_4 == skinid ||
				rows[j].skinid_5 == skinid)
			{
				res.end("already have");
				existskin = true;
				break;
			}

			rid = rows[j].rid;
			console.log("rid = " + rid);
			if(rows[j].skinid_1 == null)
			{
				key1 = "skinid_1";
				key2 = "gettime_1";
				break;
			}
			else if(rows[j].skinid_2 == null)
			{
				key1 = "skinid_2"
				key2 = "gettime_2";
				break;
			}
			else if(rows[j].skinid_3 == null)
			{
				key1 = "skinid_3";
				key2 = "gettime_3";
				break;
			}
			else if(rows[j].skinid_4 == null)
			{
				key1 = "skinid_4";
				key2 = "gettime_4";
				break;
			}
			else if(rows[j].skinid_5 == null)
			{
				key1 = "skinid_5";
				key2 = "gettime_5";
				break;
			}

		}

		if(key1 == "default" && !existskin)
		{
			connection.query('insert into skin(userid, skinid_1, gettime_1) values(?, ?, sysdate())', [userid, skinid], function(error, results)
			{
				console.log("insert");
				if(error) {console.log(error);}
				console.log(JSON.stringify(results));
			});
		}
		else if(key1 != "default" && !existskin)
		{
	        	connection.query('update skin set ' + key1 + '=?, ' + key2 + '=sysdate() where rid=?', [skinid, rid], function(error, results)
        		{	
				console.log("update");
		                if(error){console.log(error);}
				console.log(JSON.stringify(results));
			});
		}

		connection.query('select * from skin where userid=?', userid, function(error, results)
		{
			console.log("select");
			if(error) {console.log(error);}
			res.end(JSON.stringify(results));
		});
	});
});
