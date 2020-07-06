package com.useorigin.insurance.api.risk.domain;

public class ScoreAuto implements Score {

    private Integer score;

    public ScoreAuto(Integer score) {
        this.score = score;
    }

    @Override
    public ScoreAuto deduct(Integer score) {
        return new ScoreAuto(this.score - score);
    }

    @Override
    public ScoreAuto add(Integer score) {
        return new ScoreAuto(this.score + score);
    }

    @Override
    public Integer getValue() {
        return score;
    }


}
