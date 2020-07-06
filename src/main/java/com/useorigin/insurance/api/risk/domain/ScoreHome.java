package com.useorigin.insurance.api.risk.domain;

public class ScoreHome implements Score {

    private Integer score;

    public ScoreHome(Integer score) {
        this.score = score;
    }

    @Override
    public ScoreHome deduct(Integer score) {
        return new ScoreHome(this.score - score);
    }

    @Override
    public ScoreHome add(Integer score) {
        return new ScoreHome(this.score + score);
    }

    @Override
    public Integer getValue() {
        return score;
    }
}
