package com.renest.beans;

import com.renest.httpclient.Connect4Connector;

import javax.annotation.PostConstruct;
import javax.faces.bean.ApplicationScoped;
import javax.faces.bean.ManagedBean;
import javax.faces.bean.ViewScoped;
import java.util.LinkedList;
import java.util.List;
import java.util.stream.Collectors;

@ManagedBean
@ApplicationScoped
public class Connect4Bean {
    private Connect4Connector connect4Connector;

    private byte column=0;
    private String logs="Game started";

    @PostConstruct
    public void Init() {
        connect4Connector = new Connect4Connector();
        restartGame();
    }

    public byte getColumn() {
        return column;
    }

    public void setColumn(byte column) {
        this.column = column;
    }

    public String getLogs() {
        return logs;
    }

    public void setLogs(String logs) {
        this.logs = logs;
    }

    public void restartGame(){
        connect4Connector.startGame();
        logs="Game restarted";
    }

    public byte[][] getBoard() {
        return connect4Connector.getBoard();
    }

    public void doTurn() {
        int result=connect4Connector.doTurn((byte) (column-1));
        switch(result){
            case -1:
            logs="Unable to set Stone into column "+column;
            break;
            case 0:
                logs="New Stone set into column "+column;
                break;
            case 1:
                logs="Player 1 won the game!";
                break;
            case 2:
                logs="Player 2 won the game!";
                break;
        }
    }
}