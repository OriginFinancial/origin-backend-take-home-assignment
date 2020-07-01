package com.useorigin.insurance.api.risk.domain;

public class ScoreLife implements Score {

    private Integer score;

    public ScoreLife(Integer score) {
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
