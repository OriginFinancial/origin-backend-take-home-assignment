package com.useorigin.insurance.api.risk.domain;

public interface Score {
    Score deduct(Integer score);

    Score add(Integer score);

    Integer getScore();
}
