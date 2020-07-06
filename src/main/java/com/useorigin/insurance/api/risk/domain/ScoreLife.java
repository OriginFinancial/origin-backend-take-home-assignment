package com.useorigin.insurance.api.risk.domain;

public class ScoreLife implements Score {

    private Integer score;

    public ScoreLife(Integer score) {
        this.score = score;
    }

    @Override
    public ScoreLife deduct(Integer score) {
        return new ScoreLife(this.score - score);
    }

    @Override
    public ScoreLife add(Integer score) {
        return new ScoreLife(this.score + score);
    }

    @Override
    public Integer getValue() {
        return score;
    }
}
