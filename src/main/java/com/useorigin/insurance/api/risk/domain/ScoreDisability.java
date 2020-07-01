package com.useorigin.insurance.api.risk.domain;

public class ScoreDisability implements Score {

    private Integer score;

    public ScoreDisability(Integer score) {
        this.score = score;
    }

    @Override
    public ScoreDisability deduct(Integer score) {
        return new ScoreDisability(this.score - score);
    }

    @Override
    public ScoreDisability add(Integer score) {
        return new ScoreDisability(this.score + score);
    }

    @Override
    public Integer getScore() {
        return score;
    }
}
