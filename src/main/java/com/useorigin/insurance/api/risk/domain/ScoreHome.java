package com.useorigin.insurance.api.risk.domain;

public class ScoreHome implements Score {

    private Integer score;

    public ScoreHome(Integer score) {
        this.score = score;
    }

    @Override
    public Integer deduct(Integer score) {
        return this.score - score;
    }

    @Override
    public Integer add(Integer score) {
        return this.score + score;
    }
}
