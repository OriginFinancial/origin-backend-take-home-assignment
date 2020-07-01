package com.useorigin.insurance.api.risk.domain;

public class RiskScore {

    private ScoreAuto scoreAuto;
    private ScoreDisability scoreDisability;
    private ScoreHome scoreHome;
    private ScoreLife scoreLife;

    public RiskScore(ScoreAuto scoreAuto, ScoreDisability scoreDisability, ScoreHome scoreHome, ScoreLife scoreLife) {
        this.scoreAuto = scoreAuto;
        this.scoreDisability = scoreDisability;
        this.scoreHome = scoreHome;
        this.scoreLife = scoreLife;
    }

    public static RiskScore of(ScoreAuto scoreAuto, ScoreDisability scoreDisability, ScoreHome scoreHome, ScoreLife scoreLife) {
        return new RiskScore(scoreAuto, scoreDisability, scoreHome, scoreLife);
    }

    public ScoreAuto getScoreAuto() {
        return scoreAuto;
    }

    public ScoreDisability getScoreDisability() {
        return scoreDisability;
    }

    public ScoreHome getScoreHome() {
        return scoreHome;
    }

    public ScoreLife getScoreLife() {
        return scoreLife;
    }
}
