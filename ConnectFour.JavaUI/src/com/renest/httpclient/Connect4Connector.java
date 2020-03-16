package com.renest.httpclient;

import com.google.gson.reflect.TypeToken;

public class Connect4Connector extends HttpConnector {
    public Connect4Connector() {
        super(Connection.BASE_URL+"Connect4Game/");
    }

    public byte[][] getBoard() {
        HttpResponse<byte[][]> result = super.doGet("", null, new TypeToken<byte[][]>() {
        }.getType());
        if(result.getStatusCode()!=200){
            System.err.println(result.getReasonPhrase());
            return null;
        }
        return result.getData();
    }

    public byte doTurn(byte column) {
        HttpResponse<Byte> result = super.doPost(column+"", null,null, new TypeToken<Byte>() {
        }.getType());
        if(result.getStatusCode()!=200){
            System.err.println(result.getReasonPhrase());
            return -1;
        }
        return result.getData();
    }

    public void startGame() {
        HttpResponse<byte[][]> result = super.doPut("", null,null, null);
    }
}
