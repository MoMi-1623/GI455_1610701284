const { CONNECTING } = require('ws');
var websocket = require('ws');

var callbackInitServer = ()=>
{
    console.log("server is running.");
}

var wss = new websocket.Server({port:1623} , callbackInitServer)

var wslist = [];



wss.on("connection",(ws)=>
{   
    wslist.push(ws);
    for(var i = 0;i<wslist.length;i++)
    {
        if(wslist[i] == ws)
        {
            console.log("client " + i + " : connected");
            break;
        }
    }
    ws.on("message", (data)=>
    {
        for(var i = 0;i<wslist.length;i++)
        {
            if(wslist[i] != ws)
            {
                wslist[i].send(data + "                                               ");
            }
            if(wslist[i] == ws)
            {
                wslist[i].send(data);
                console.log("sent from " + i + " " + data);
            }
        }
    });

    ws.on("close", ()=>
    {
        for(var i = 0;i<wslist.length;i++)
        {
            if(wslist[i] == ws)
            {
                console.log("client " + i + " disconnected");
                wslist = RemoveArray(wslist,ws)
                break;
            }
        }
    });

});


function RemoveArray(arr, value)
{
    return arr.filter((element)=>
    {
        return element != value;
    });
}

function Boardcast(data)
{
    for(var i = 0;i<wslist.length;i++)
    {       
            wslist[i].send(data);
    }
}


