package com.useorigin.insurance.api.risk.domain;

public class ScoreAuto implements Score {

    private Integer score;

    public ScoreAuto(Integer score) {
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
