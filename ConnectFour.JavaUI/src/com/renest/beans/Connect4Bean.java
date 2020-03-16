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

    public void restartGame(){
        connect4Connector.startGame();
    }

    public byte[][] getBoard() {
        return connect4Connector.getBoard();
    }

    public void doTurn() {
        int result=connect4Connector.doTurn((byte) (column-1));
    }
}